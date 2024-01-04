using System;
using Cysharp.Threading.Tasks;
using DataPersistance.Data.ScriptableObjects;
using DG.Tweening;
using FSM;
using FSM.Enemy;
using FSM.Enemy.States;
using Gameplay.Player.DamageDealers;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;
using Microlight.MicroBar;

namespace Gameplay.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] private MicroBar hpBar;

        public EnemySO enemySettings;
        public Player.Player player;
        public Transform[] waypoints;

        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public Animator animator;
        [HideInInspector] public int currentWaypoint;
        [HideInInspector] public bool waypointReached;
        [HideInInspector] public bool canMoveNextWaypoint;
        [HideInInspector] public float lastAttackTime;

        private int currentHealth;
        private StateMachine<EnemyState> EnemyFSM;
        private SkinnedMeshRenderer meshRenderer;
        private LayerMask playerMask;
        private bool playerDetected;
        private BoxCollider boxCollider;


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

            EnemyFSM.AddState(EnemyState.Idle, new IdleState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Patrol, new PatrolState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Chase, new ChaseState(this, EnemyFSM));
            EnemyFSM.AddState(EnemyState.Attack, new AttackState(this, EnemyFSM, needsExitTime: true));

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
            playerDetected = Physics.CheckSphere(transform.position, enemySettings.sphereRadius, playerMask,
                QueryTriggerInteraction.Collide);
        }

        public void TakeDamage(int damage)
        {
            StartTakeDamageAnim();
            CheckHealth(damage);
            hpBar.UpdateHealthBar(currentHealth);
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
            animator.SetTrigger(AnimationParameters.TakeDamage);
            meshRenderer.material.DOColor(Color.red, .5f).From().SetEase(Ease.InFlash);
        }

        private void CheckHealth(int damage)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private async void Die()
        {
            // deactivate hp bar and collider for prevent further attacks
            boxCollider.enabled = false;
            //hpBar.FadeBar(true, 1f);
            hpBar.gameObject.SetActive(false);
            
            // play animation
            animator.SetTrigger(AnimationParameters.Die);
            var dieAnimation = animator.GetCurrentAnimatorStateInfo(0);
            await UniTask.WaitForSeconds(dieAnimation.length / dieAnimation.speed);
            
            // fade out animation
            await meshRenderer.material.DOFade(0f, 1f).ToUniTask();

            Destroy(this.gameObject, 1f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, enemySettings.sphereRadius);
        }
    }
}