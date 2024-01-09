using System;
using Cysharp.Threading.Tasks;
using DataPersistance.Data.ScriptableObjects;
using DG.Tweening;
using FSM;
using FSM.Enemy;
using FSM.Enemy.States;
using Gameplay.Interfaces;
using Gameplay.Player.DamageDealers;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;
using Microlight.MicroBar;

namespace Gameplay.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable, IDealDamage
    {
        public MicroBar hpBar;
        public EnemySO enemySettings;
        public Player.Player player;
        public Transform[] waypoints;

        [HideInInspector] public SkinnedMeshRenderer meshRenderer;
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public BoxCollider boxCollider;
        [HideInInspector] public Animator animator;
        [HideInInspector] public float lastAttackTime;
        [HideInInspector] public bool waypointReached;
        [HideInInspector] public bool canMoveNextWaypoint;
        [HideInInspector] public int currentWaypoint;
        [HideInInspector] public int currentHealth;

        private StateMachine<EnemyState> EnemyFSM;
        private LayerMask playerMask;
        [HideInInspector] public bool playerDetected;

        public TMP_Text stateText;

        private void Awake()
        {
            currentWaypoint = 0;
            lastAttackTime = 0;
            currentHealth = enemySettings.enemyBaseHealth;
            waypointReached = false;
            playerDetected = false;
            canMoveNextWaypoint = true;
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider>();
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            playerMask = LayerMask.GetMask("Player");
            hpBar.Initialize(currentHealth);

            EnemyFSM = new StateMachine<EnemyState>();

            var takeDamage = new TakeDamageState(this, EnemyFSM);
            
            EnemyFSM.AddState(EnemyState.Idle, new IdleState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Patrol, new PatrolState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Chase, new ChaseState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Attack, new AttackState(this, EnemyFSM, needsExitTime: true));
            EnemyFSM.AddState(EnemyState.Die, new DieState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.TakeDamage, takeDamage
                .AddAction<int>("OnHit", (int damage) => takeDamage.OnHit(damage)));
            
            EnemyFSM.AddTriggerTransitionFromAny("TakeDamage", EnemyState.TakeDamage, forceInstantly: true);
            EnemyFSM.AddTransitionFromAny(EnemyState.Die, t => currentHealth <= 0);
            EnemyFSM.AddTransition(EnemyState.Idle, EnemyState.Patrol, t => canMoveNextWaypoint);
            EnemyFSM.AddTransition(EnemyState.Idle, EnemyState.Chase, t => playerDetected);
            EnemyFSM.AddTransition(EnemyState.Patrol, EnemyState.Chase, t => playerDetected);
            EnemyFSM.AddTransition(EnemyState.Patrol, EnemyState.Idle, t => waypointReached);
            //EnemyFSM.AddTransition(EnemyState.Chase, EnemyState.Patrol, t => !playerDetected);
            EnemyFSM.AddTriggerTransition("OnAttack", new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack));
            EnemyFSM.AddTransition(EnemyState.Attack, EnemyState.Chase, t => playerDetected);
            //EnemyFSM.AddTransition(EnemyState.Attack, EnemyState.Patrol, t => !playerDetected);
            EnemyFSM.AddTransition(EnemyState.TakeDamage, EnemyState.Chase, t => playerDetected);
            //EnemyFSM.AddTransition(EnemyState.TakeDamage, EnemyState.Patrol, t => !playerDetected);

            EnemyFSM.SetStartState(EnemyState.Patrol);
            EnemyFSM.Init();
        }

        private void Update()
        {
            EnemyFSM.OnLogic();

            stateText.SetText(EnemyFSM.GetActiveHierarchyPath().Split('/')[1]);
            playerDetected = Physics.CheckSphere(transform.position, enemySettings.sphereRadius, playerMask,
                QueryTriggerInteraction.Collide);
            hpBar.transform.position = transform.position;
        }

        #region IDamageable Functions

        public void TakeDamage(int damage)
        {
            EnemyFSM.Trigger("TakeDamage");
            EnemyFSM.OnAction<int>("OnHit", damage);
        }
        
        #endregion

        #region IDealDamage Functions

        public void StartDealDamage()
        {
            GameplayEvents.StartDealDamage.Invoke(this.gameObject);
        }

        public void EndDealDamage()
        {
            GameplayEvents.EndDealDamage.Invoke(this.gameObject);
        }

        #endregion
        

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, enemySettings.sphereRadius);
        }
    }
}