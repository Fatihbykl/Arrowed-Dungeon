using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FSM;
using FSM.Enemy;
using FSM.Enemy.States;
using Gameplay.Player.DamageDealers;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Gameplay.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public Player.Player player;
        public float sphereRadius;
        public Transform[] waypoints;
        [HideInInspector] public int currentWaypoint;
        [HideInInspector] public bool waypointReached;
        [HideInInspector] public bool canMoveNextWaypoint;
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Animator animator;
        public float patrolSpeed;
        public float chaseSpeed;
        public float waypointWaitTime;
        public float attackDelay;

        private StateMachine<EnemyState> EnemyFSM;
        private LayerMask playerMask;
        private bool playerDetected;
        private SkinnedMeshRenderer meshRenderer;
        
        
        public TMP_Text stateText;
        

        private void Awake()
        {
            currentWaypoint = 0;
            waypointReached = false;
            playerDetected = false;
            canMoveNextWaypoint = true;
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            playerMask = LayerMask.GetMask("Player");
            
            EnemyFSM = new StateMachine<EnemyState>();
            
            EnemyFSM.AddState(EnemyState.Idle, new IdleState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Patrol, new PatrolState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Chase, new ChaseState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Attack, new AttackState(this, EnemyFSM, needsExitTime:true));
            
            EnemyFSM.AddTransition(EnemyState.Idle, EnemyState.Patrol, t => canMoveNextWaypoint);
            EnemyFSM.AddTransition(EnemyState.Idle, EnemyState.Chase, t => playerDetected);
            EnemyFSM.AddTransition(EnemyState.Patrol, EnemyState.Chase, t => playerDetected);
            EnemyFSM.AddTransition(EnemyState.Patrol, EnemyState.Idle, t => waypointReached);
            EnemyFSM.AddTransition(EnemyState.Chase, EnemyState.Patrol, t => !playerDetected);
            EnemyFSM.AddTriggerTransition("OnAttack", new Transition<EnemyState>(EnemyState.Chase, EnemyState.Attack));
            EnemyFSM.AddTransition(EnemyState.Attack, EnemyState.Chase, t => playerDetected);
            EnemyFSM.AddTransition(EnemyState.Attack, EnemyState.Patrol, t => !playerDetected);

            
            EnemyFSM.SetStartState(EnemyState.Patrol);
            EnemyFSM.Init();
        }

        private void Update()
        {
            EnemyFSM.OnLogic();
            
            stateText.SetText(EnemyFSM.GetActiveHierarchyPath().Split('/')[1]);
            playerDetected = Physics.CheckSphere(transform.position, sphereRadius, playerMask,
                QueryTriggerInteraction.Collide);
        }

        public void TakeDamage()
        {
            animator.SetTrigger(AnimationParameters.TakeDamage);
        }
        
        public void StartDealDamage()
        {
            GameplayEvents.StartDealDamage.Invoke(this.gameObject);
        }
        
        public void EndDealDamage()
        {
            GameplayEvents.EndDealDamage.Invoke(this.gameObject);
        }
        
        public void StartTakeDamageAnim()
        {
            meshRenderer.material.DOColor(Color.red, .5f).From().SetEase(Ease.InFlash);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, sphereRadius);
        }
    }
}