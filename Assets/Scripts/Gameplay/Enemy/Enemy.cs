using DataPersistance.Data.ScriptableObjects;
using FSM.Enemy;
using FSM.Enemy.States;
using Gameplay.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;
using Microlight.MicroBar;

namespace Gameplay.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public MicroBar hpBar;
        public EnemySO enemySettings;
        public Player.Player player;
        public Transform[] waypoints;

        [HideInInspector] public SkinnedMeshRenderer meshRenderer;
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public BoxCollider boxCollider;
        [HideInInspector] public Animator animator;
        [HideInInspector] public bool waypointReached;
        [HideInInspector] public bool canMoveNextWaypoint;
        [HideInInspector] public int currentWaypoint;
        [HideInInspector] public int currentHealth ;
        [HideInInspector] public bool playerDetected;
        [HideInInspector] public float lastAttackTime;
        
        protected StateMachine<EnemyState> EnemyFSM;
        private LayerMask playerMask;

        public TMP_Text stateText;

        private void Awake()
        {
            // variables
            playerMask = LayerMask.GetMask("Player");
            lastAttackTime = 0;
            playerDetected = false;
            waypointReached = false;
            canMoveNextWaypoint = true;
            currentWaypoint = 0;
            currentHealth = enemySettings.enemyBaseHealth;
            
            // components
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider>();
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            
            // agent settings
            agent.radius = enemySettings.radius;
            agent.height = enemySettings.height;
            agent.speed = enemySettings.patrolSpeed;
            
            // hp bar initialization
            hpBar.Initialize(currentHealth);
            
            // FSM
            EnemyFSM = new StateMachine<EnemyState>();

            var takeDamage = new TakeDamageState(this, EnemyFSM);
            
            EnemyFSM.AddState(EnemyState.Idle, new IdleState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Patrol, new PatrolState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Die, new DieState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Chase, new ChaseState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.TakeDamage, takeDamage
                .AddAction<int>("OnHit", (int damage) => takeDamage.OnHit(damage)));
            
            EnemyFSM.AddTriggerTransitionFromAny("TakeDamage", EnemyState.TakeDamage, forceInstantly: true);
            EnemyFSM.AddTransitionFromAny(EnemyState.Die, t => currentHealth <= 0);
            EnemyFSM.AddTransition(EnemyState.Idle, EnemyState.Patrol, t => canMoveNextWaypoint);
            EnemyFSM.AddTransition(EnemyState.Idle, EnemyState.Chase, t => playerDetected);
            EnemyFSM.AddTransition(EnemyState.Patrol, EnemyState.Chase, t => playerDetected);
            EnemyFSM.AddTransition(EnemyState.Patrol, EnemyState.Idle, t => waypointReached);
            EnemyFSM.AddTransition(EnemyState.TakeDamage, EnemyState.Chase, t => playerDetected);
        }

        private void Start()
        {
            hpBar.transform.LookAt(hpBar.transform.position + Camera.main.transform.rotation * Vector3.back, Camera.main.transform.rotation * Vector3.back);
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

        public void TakeDamage(int damage)
        {
            EnemyFSM.Trigger("TakeDamage");
            EnemyFSM.OnAction<int>("OnHit", damage);
        }
    }
}