using System;
using System.Collections.Generic;
using System.Linq;
using Animations;
using Cysharp.Threading.Tasks;
using FSM.Enemy;
using FSM.Enemy.States;
using Gameplay.Interfaces;
using UnityEngine;
using UnityHFSM;
using NaughtyAttributes;
using DG.Tweening;
using Gameplay.AbilitySystem;
using Gameplay.DamageDealers;
using Gameplay.Managers;
using Gameplay.Movement.Common;
using Gameplay.Movement.Controllers;
using Gameplay.StatSystem;
using UI.Dynamic_Floating_Text.Scripts;
using UI.MicroBar;

namespace Gameplay.Enemy
{
    [Serializable]
    public class EnemyStats
    {
        public IntegerStat damage;
        public IntegerStat maxHealth;
        [HideInInspector] public VitalStat health;
        public FloatStat armor;
        public FloatStat chaseSpeed;

        public void InitHealth()
        {
            health.BaseValue = maxHealth.BaseValue;
            health.useUpperBound = true;
            health.upperBound = maxHealth.BaseValue;
        }
    }
    public class Enemy : MonoBehaviour, IDamageable, IHealable
    {
        [Header("Stats")]
        [HorizontalLine(color: EColor.White, height: 1f)]
        [Space(5)]
        public EnemyStats enemyStats;
        
        
        [Header("References")]
        [HorizontalLine(color: EColor.White, height: 1f)]
        [Space(5)]
        public AbilityBase[] abilities;
        public GameObject waypointObject;
        public bool isRanged;
        [ShowIf("isRanged")]
        public GameObject projectileSpawnPosition;
        public GameObject healVFXPrefab;

        public MicroBar hpBar;

        [Header("Agent Settings")]
        [HorizontalLine(color: EColor.White, height: 1f)]
        [Space(5)]
        [InfoBox("Awareness size of the enemy.")]
        public float sphereRadius;
        public float waypointWaitTime;
        public float patrolSpeed;
        public float stoppingDistance;
        
        [Header("Take Damage Blink Settings")]
        [HorizontalLine(color: EColor.White, height: 1f)]
        [Space(5)]
        public float blinkIntensity = 5f;
        public float blinkDuration = 0.2f;

        [Header("Sound Effects")] [HorizontalLine(color: EColor.White, height: 1f)] [Space(5)]
        public SoundClip[] footsteps;
        public SoundClip healSoundEffect;
        
        [HideInInspector] public Player.Player player;
        [HideInInspector] public List<AbilityHolder> abilityHolders;
        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public SkinnedMeshRenderer meshRenderer;
        [HideInInspector] public BaseAgentController agentController;
        [HideInInspector] public CapsuleCollider capsuleCollider;
        [HideInInspector] public Animator animator;
        [HideInInspector] public bool waypointReached;
        [HideInInspector] public bool canMoveNextWaypoint;
        [HideInInspector] public int currentWaypoint;
        [HideInInspector] public bool playerDetected;
        [HideInInspector] public bool castingAbility;
        [HideInInspector] public bool isInStatusEffect;
        [HideInInspector] public LayerMask playerMask;

        private StateMachine<EnemyState> EnemyFSM;
        private WeaponDamageDealer damageDealer;
        private AudioSource _audioSource;
        
        // Events
        public event Action<GameObject> RangedAutoAttackEvent;
        public event Action<GameObject> LineAttackHitEvent;
        public event Action<GameObject> JumpAttackJumpEvent;
        public event Action<GameObject> JumpAttackLandEvent;

        private void Awake()
        {
            // variables
            playerMask = LayerMask.GetMask("Player");
            
            playerDetected = false;
            waypointReached = false;
            canMoveNextWaypoint = true;
            currentWaypoint = 0;
            
            // event
            enemyStats.chaseSpeed.StatChanged += OnSpeedStatChanged;
            enemyStats.health.StatChanged += OnHealthChanged;
            
            // components
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            damageDealer = GetComponentInChildren<WeaponDamageDealer>();
            agentController = GetComponent<BaseAgentController>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            _audioSource = GetComponent<AudioSource>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();

            // hp bar initialization
            enemyStats.InitHealth();
            hpBar.Initialize(enemyStats.health.BaseValue);
            
            // FSM
            EnemyFSM = new StateMachine<EnemyState>();
            
            EnemyFSM.AddState(EnemyState.Idle, new IdleState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Patrol, new PatrolState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Die, new DieState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Chase, new ChaseState(this, EnemyFSM));
            EnemyFSM.AddTransitionFromAny(EnemyState.Die, t => enemyStats.health.Value <= 0, forceInstantly: true);
            EnemyFSM.AddTriggerTransitionFromAny("TriggerDie", EnemyState.Die);
            EnemyFSM.AddTransition(EnemyState.Idle, EnemyState.Patrol, t => canMoveNextWaypoint);
            EnemyFSM.AddTransition(EnemyState.Idle, EnemyState.Chase, t => playerDetected);
            EnemyFSM.AddTransition(EnemyState.Patrol, EnemyState.Chase, t => playerDetected);
            EnemyFSM.AddTransition(EnemyState.Patrol, EnemyState.Idle, t => waypointReached);
            
            EnemyFSM.SetStartState(EnemyState.Patrol);
        }


        private void Start()
        {
            EnemyFSM.Init();

            player = GameManager.Instance.playerObject.GetComponent<Player.Player>();
            PrepareAbilities();

            hpBar.transform.LookAt(hpBar.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.back);
        }

        private void PrepareAbilities()
        {
            if (abilities == null) { return; }
            foreach (var ability in abilities)
            {
                var holder = gameObject.AddComponent<AbilityHolder>();
                holder.ability = Instantiate(ability);
                holder.ability.OnCreate(gameObject);
                holder.target = player.gameObject;
            }
            agentController.agent.stoppingDistance = stoppingDistance;
            abilityHolders = GetComponents<AbilityHolder>().ToList();
        }

        private void FixedUpdate()
        {
            animator.SetFloat(AnimationParameters.Speed, rb.velocity.onlyXZ().magnitude);
        }

        private void Update()
        {
            EnemyFSM.OnLogic();
            
            playerDetected = Physics.CheckSphere(transform.position, sphereRadius, playerMask,
                QueryTriggerInteraction.Collide);
            
            hpBar.transform.position =
                new Vector3(transform.position.x - 0.5f, hpBar.transform.position.y, transform.position.z);
        }

        public void TakeDamage(int damage)
        {
            playerDetected = true;
            damage = Mathf.RoundToInt(damage * (1 - enemyStats.armor.Value / 100f));
            
            enemyStats.health.AddModifier(new StatModifier(-damage, StatModType.Flat));
            CreateFloatingText(damage.ToString(), DynamicTextManager.Instance.EnemyDamageData);
            if (enemyStats.health.Value > 0) { StartTakeDamageAnim(); }
        }

        private void CreateFloatingText(string damage, DynamicTextData data)
        {
            if (damage == "0") { damage = "MISS!"; }
            
            DynamicTextManager.Instance.CreateText(transform, damage, data);
        }

        private async void StartTakeDamageAnim()
        {
            //gameObject.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.1f);
            await meshRenderer.material.DOColor(Color.white * blinkIntensity, blinkDuration / 2).ToUniTask();
            await meshRenderer.material.DOColor(Color.white, blinkDuration / 2).ToUniTask();
        }

        public void Heal(int healthAmount)
        {
            var particle = Instantiate(healVFXPrefab, transform);
            Destroy(particle, 1f);
            CreateFloatingText(healthAmount.ToString(), DynamicTextManager.Instance.EnemyHealData);
            enemyStats.health.AddModifier(new StatModifier(healthAmount, StatModType.Flat));
            AudioManager.Instance.PlaySoundFXClip(healSoundEffect, transform);
        }

        public void StartDealDamage()
        {
            damageDealer.OnStartDealDamage(enemyStats.damage.Value);
        }

        public void EndDealDamage()
        {
            damageDealer.OnEndDealDamage();
        }

        public void TriggerDie()
        {
            EnemyFSM.Trigger("TriggerDie");
        }

        public void SendProjectile()
        {
            RangedAutoAttackEvent?.Invoke(gameObject);
        }

        public void LineAttackHit()
        {
            LineAttackHitEvent?.Invoke(gameObject);
        }

        public void JumpAttackJump()
        {
            JumpAttackJumpEvent?.Invoke(gameObject);
        }

        public void JumpAttackLand()
        {
            JumpAttackLandEvent?.Invoke(gameObject);
        }

        public void Footsteps()
        {
            AudioManager.Instance.PlayRandomSoundFXWithSource(_audioSource, footsteps);
        }

        private void OnSpeedStatChanged()
        {
            agentController.speed = enemyStats.chaseSpeed.Value;
        }

        private void OnHealthChanged()
        {
            hpBar.UpdateHealthBar(enemyStats.health.Value);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, sphereRadius);
        }
    }
}