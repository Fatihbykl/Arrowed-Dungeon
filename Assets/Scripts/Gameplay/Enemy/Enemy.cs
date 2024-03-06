using System;
using System.Collections.Generic;
using System.Linq;
using AbilitySystem;
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

namespace Gameplay.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [Header("Needed Objects")]
        [HorizontalLine(color: EColor.White, height: 1f)]
        [Space(10)]
        
        [Foldout("Base Enemy Settings")] public MicroBar hpBar;
        [Foldout("Base Enemy Settings")] public EnemySO enemySettings;
        [Foldout("Base Enemy Settings")] public Player.Player player;
        [Foldout("Base Enemy Settings")] public Transform[] waypoints;
        [Foldout("Base Enemy Settings")] public bool canKnockbackable;
        [Foldout("Base Enemy Settings")] public AbilityBase[] abilities;
        public List<AbilityHolder> abilityHolders;
        
        [Header("Take Damage Emission Settings")]
        [HorizontalLine(color: EColor.White, height: 1f)]
        [Space(10)]
        [Foldout("Base Enemy Settings")] public float blinkIntensity = 10f;
        [Foldout("Base Enemy Settings")] public float blinkDuration = 2f;
        
        [Header("Test Variables")]
        [HorizontalLine(color: EColor.White, height: 1f)]
        [Space(10)]
        [Foldout("Base Enemy Settings")] public TMP_Text stateText;
        [Space(10)]
        
        protected StateMachine<EnemyState> EnemyFSM;
        private LayerMask playerMask;
        private WeaponDamageDealer damageDealer;
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
        
        private void Awake()
        {
            // variables
            playerMask = LayerMask.GetMask("Player");
            rb = GetComponent<Rigidbody>();
            playerDetected = false;
            waypointReached = false;
            canMoveNextWaypoint = true;
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

            foreach (var ability in abilities)
            {
                var holder = gameObject.AddComponent<AbilityHolder>();
                holder.ability = ability;
                holder.owner = gameObject;
                holder.target = player.gameObject;
            }

            abilityHolders = GetComponents<AbilityHolder>().ToList();
            
            hpBar.transform.LookAt(hpBar.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.back);
        }

        private void FixedUpdate()
        {
            animator.SetFloat(AnimationParameters.Speed, rb.velocity.magnitude);
        }

        private void Update()
        {
            EnemyFSM.OnLogic();
            
            stateText.SetText(EnemyFSM.GetActiveHierarchyPath().Split('/')[1]);
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
            if (canKnockbackable) { rb.AddForce(direction, ForceMode.Impulse); }
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
            damageDealer.OnStartDealDamage(this.gameObject);
        }

        public void EndDealDamage()
        {
            damageDealer.OnEndDealDamage(this.gameObject);
        }
    }
}