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
        [HideInInspector] public IntegerStat health;
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
        //public EnemySO enemySettings;
        public EnemyStats enemyStats;
        
        
        [Header("References")]
        [HorizontalLine(color: EColor.White, height: 1f)]
        [Space(10)]
        public AbilityBase[] abilities;
        public bool isRanged;
        [ShowIf("isRanged")]
        public GameObject projectileSpawnPosition;
        public MicroBar hpBar;
        public Transform[] waypoints;
        
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
            EnemyFSM.AddTransition(EnemyState.Idle, EnemyState.Patrol, t => canMoveNextWaypoint);
            EnemyFSM.AddTransition(EnemyState.Idle, EnemyState.Chase, t => playerDetected);
            EnemyFSM.AddTransition(EnemyState.Patrol, EnemyState.Chase, t => playerDetected);
            EnemyFSM.AddTransition(EnemyState.Patrol, EnemyState.Idle, t => waypointReached);
            
            EnemyFSM.SetStartState(EnemyState.Patrol);
        }

        

        private void Start()
        {
            EnemyFSM.Init();

            PrepareAbilities();
            player = GameManager.instance.playerObject.GetComponent<Player.Player>();

            hpBar.transform.LookAt(hpBar.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.back);
        }

        private void PrepareAbilities()
        {
            if (abilities == null) { return; }
            foreach (var ability in abilities)
            {
                var holder = this.gameObject.AddComponent<AbilityHolder>();
                holder.ability = Instantiate(ability);
                holder.owner = this.gameObject;
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
            hpBar.UpdateHealthBar(enemyStats.health.Value);
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

        public void SendProjectile()
        {
            RangedAutoAttack.rangedAutoAttackEvent.Invoke(gameObject);
        }
        
        private void OnSpeedStatChanged()
        {
            agentController.speed = enemyStats.chaseSpeed.Value;
        }

        public void Heal(int healthAmount)
        {
            var newHealth = enemyStats.health.Value + healthAmount;
            //newHealth = newHealth <= enemyStats.health.baseValue ? newHealth : enemyStats.health.baseValue;
            enemyStats.health.AddModifier(new StatModifier(newHealth, StatModType.Flat));
        }
    }
}