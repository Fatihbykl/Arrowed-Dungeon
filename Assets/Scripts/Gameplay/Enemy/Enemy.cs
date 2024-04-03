using System;
using System.Collections.Generic;
using System.Linq;
using AbilitySystem;
using AbilitySystem.NPC;
using Cysharp.Threading.Tasks;
using DataPersistance.Data.ScriptableObjects;
using FSM.Enemy;
using FSM.Enemy.States;
using Gameplay.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;
using Microlight.MicroBar;
using NaughtyAttributes;
using DG.Tweening;
using ECM.Controllers;
using FSM;
using Gameplay.DamageDealers;
using Managers;

namespace Gameplay.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [Header("References")]
        [HorizontalLine(color: EColor.White, height: 1f)]
        [Space(10)]
        public EnemySO enemySettings;
        public GameObject projectileSpawnPosition;
        public MicroBar hpBar;
        public Player.Player player;
        public Transform[] waypoints;

        [Header("Take Damage Emission Settings")]
        [HorizontalLine(color: EColor.White, height: 1f)]
        [Space(10)]
        public float blinkIntensity = 10f;
        public float blinkDuration = 2f;
        
        [HideInInspector] public List<AbilityHolder> abilityHolders;
        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public SkinnedMeshRenderer meshRenderer;
        [HideInInspector] public BaseAgentController agentController;
        [HideInInspector] public CapsuleCollider capsuleCollider;
        [HideInInspector] public Animator animator;
        [HideInInspector] public bool waypointReached;
        [HideInInspector] public bool canMoveNextWaypoint;
        [HideInInspector] public int currentWaypoint;
        [HideInInspector] public int currentHealth ;
        [HideInInspector] public bool playerDetected;
        [HideInInspector] public bool castingAbility;
        [HideInInspector] public bool letAIManagerSetDestination;
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
            currentHealth = enemySettings.enemyBaseHealth;
            damageDealer = GetComponentInChildren<WeaponDamageDealer>();
            
            // components
            agentController = GetComponent<BaseAgentController>();
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            
            // agentController settings
            // agentController.radius = enemySettings.radius;
            // agentController.height = enemySettings.height;
            //agentController.agent.speed = enemySettings.patrolSpeed;
            //agentController.agent.stoppingDistance = enemySettings.stoppingDistance;
            
            // hp bar initialization
            hpBar.Initialize(currentHealth);
            
            // FSM
            EnemyFSM = new StateMachine<EnemyState>();
            
            EnemyFSM.AddState(EnemyState.Idle, new IdleState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Patrol, new PatrolState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Die, new DieState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Chase, new ChaseState(this, EnemyFSM));
            EnemyFSM.AddTransitionFromAny(EnemyState.Die, t => currentHealth <= 0, forceInstantly: true);
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

            hpBar.transform.LookAt(hpBar.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.back);
        }

        private void PrepareAbilities()
        {
            if (enemySettings.abilities == null) { return; }
            foreach (var ability in enemySettings.abilities)
            {
                var holder = this.gameObject.AddComponent<AbilityHolder>();
                holder.ability = Instantiate(ability);
                holder.owner = this.gameObject;
                holder.target = player.gameObject;
            }
            agentController.agent.stoppingDistance = enemySettings.stoppingDistance;
            abilityHolders = GetComponents<AbilityHolder>().ToList();
        }

        private void FixedUpdate()
        {
            animator.SetFloat(AnimationParameters.Speed, rb.velocity.magnitude);
        }

        private void Update()
        {
            EnemyFSM.OnLogic();
            
            playerDetected = Physics.CheckSphere(transform.position, enemySettings.sphereRadius, playerMask,
                QueryTriggerInteraction.Collide);
            
            hpBar.transform.position =
                new Vector3(transform.position.x - 0.5f, hpBar.transform.position.y, transform.position.z);
        }

        public void TakeDamage(int damage, Vector3 direction)
        {
            currentHealth -= damage;
            hpBar.UpdateHealthBar(currentHealth);
            playerDetected = true;
            if (enemySettings.canKnockbackable) { rb.AddForce(direction, ForceMode.Impulse); }
            if (currentHealth > 0) { StartTakeDamageAnim(); }
        }
        
        private async void StartTakeDamageAnim()
        {
            gameObject.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.1f);
            await meshRenderer.material.DOColor(Color.white * blinkIntensity, blinkDuration / 2).ToUniTask();
            await meshRenderer.material.DOColor(Color.white, blinkDuration / 2).ToUniTask();
        }
        
        public void StartDealDamage()
        {
            damageDealer.OnStartDealDamage(enemySettings.enemyBaseDamage);
        }

        public void EndDealDamage()
        {
            damageDealer.OnEndDealDamage();
        }

        public void SendProjectile()
        {
            var rangedAutoAttack = abilityHolders
                .FirstOrDefault(a => String.Equals(a.ability.name, "RangedAutoAttack(Clone)"))
                ?.ability as RangedAutoAttack;

            if (rangedAutoAttack != null) rangedAutoAttack.SendProjectile();
            else
            {
                Debug.LogError("RangedAutoAttack(Clone) not found!");
            }
        }
        
        private void OnDestroy()
        {
            AIManager.Instance.RemoveUnit(this);
        }
    }
}