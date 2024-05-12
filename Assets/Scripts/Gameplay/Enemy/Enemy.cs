using System;
using System.Collections.Generic;
using System.Linq;
using AbilitySystem;
using AbilitySystem.NPC;
using Cysharp.Threading.Tasks;
using FSM.Enemy;
using FSM.Enemy.States;
using Gameplay.Interfaces;
using UnityEngine;
using UnityHFSM;
using Microlight.MicroBar;
using NaughtyAttributes;
using DG.Tweening;
using ECM.Controllers;
using FSM;
using Gameplay.DamageDealers;
using Managers;
using StatSystem;

namespace Gameplay.Enemy
{
    [Serializable]
    public class EnemyStats
    {
        public IntegerStat damage;
        public IntegerStat maxHealth;
        [HideInInspector] public VitalStat health;
        public IntegerStat armor;
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
        [Space(10)]
        public EnemyStats enemyStats;
        
        
        [Header("References")]
        [HorizontalLine(color: EColor.White, height: 1f)]
        [Space(10)]
        public AbilityBase[] abilities;
        public Transform[] waypoints;
        public bool isRanged;
        [ShowIf("isRanged")]
        public GameObject projectileSpawnPosition;
        public GameObject healVFXPrefab;

        public MicroBar hpBar;

        [Header("Agent Settings")]
        [HorizontalLine(color: EColor.White, height: 1f)]
        [Space(10)]
        [InfoBox("Awareness size of the enemy.")]
        public float sphereRadius;
        public float waypointWaitTime;
        public float patrolSpeed;
        public float stoppingDistance;
        
        [Header("Take Damage Blink Settings")]
        [HorizontalLine(color: EColor.White, height: 1f)]
        [Space(10)]
        public float blinkIntensity = 5f;
        public float blinkDuration = 0.2f;
        
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
        [HideInInspector] public bool letAIManagerSetDestination;
        [HideInInspector] public bool isInStatusEffect;
        [HideInInspector] public LayerMask playerMask;

        private StateMachine<EnemyState> EnemyFSM;
        private WeaponDamageDealer damageDealer;
        
        // Events
        public event Action<GameObject> RangedAutoAttackEvent;
        public event Action<GameObject> LineAttackHitEvent;
        public event Action<GameObject> JumpAttackJumpEvent;
        public event Action<GameObject> JumpAttackLandEvent;

        private void Awake()
        {
            // variables
            playerMask = LayerMask.GetMask("Player");
            rb = GetComponent<Rigidbody>();
            playerDetected = false;
            waypointReached = false;
            canMoveNextWaypoint = true;
            letAIManagerSetDestination = true;
            currentWaypoint = 0;
            damageDealer = GetComponentInChildren<WeaponDamageDealer>();
            
            // event
            enemyStats.chaseSpeed.StatChanged += OnSpeedStatChanged;
            enemyStats.health.StatChanged += OnHealthChanged;
            
            // components
            agentController = GetComponent<BaseAgentController>();
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            
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

            player = GameManager.instance.playerObject.GetComponent<Player.Player>();
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
            animator.SetFloat(AnimationParameters.Speed, rb.velocity.magnitude);
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
            enemyStats.health.AddModifier(new StatModifier(-damage, StatModType.Flat));
            playerDetected = true;
            if (enemyStats.health.Value > 0) { StartTakeDamageAnim(); }
        }

        private async void StartTakeDamageAnim()
        {
            //gameObject.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.1f);
            await meshRenderer.material.DOColor(Color.white * blinkIntensity, blinkDuration / 2).ToUniTask();
            await meshRenderer.material.DOColor(Color.white, blinkDuration / 2).ToUniTask();
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

        private void OnSpeedStatChanged()
        {
            agentController.speed = enemyStats.chaseSpeed.Value;
        }

        private void OnHealthChanged()
        {
            hpBar.UpdateHealthBar(enemyStats.health.Value);
        }

        public void Heal(int healthAmount)
        {
            var particle = Instantiate(healVFXPrefab, transform);
            Destroy(particle, 1f);
            enemyStats.health.AddModifier(new StatModifier(healthAmount, StatModType.Flat));
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 2.4f);
        }
    }
}