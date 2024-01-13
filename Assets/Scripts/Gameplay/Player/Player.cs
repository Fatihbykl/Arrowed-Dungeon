using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FSM;
using FSM.Player;
using FSM.Player.States;
using Gameplay.Interfaces;
using Gameplay.Player.DamageDealers;
using Microlight.MicroBar;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityHFSM;

namespace Gameplay.Player
{
    public class Player : MonoBehaviour, IDamageable
    {
        public int playerHealth;
        public MicroBar hpBar;
        [SerializeField] private Material animMaterial;
        [SerializeField] private float attackRangeRadius;
        [SerializeField] private LayerMask detectableLayersForAttack;
        [HideInInspector] public GameObject currentTarget;
        public TMP_Text stateText;

        private StateMachine<PlayerState> PlayerFSM;
        private SkinnedMeshRenderer[] renderers;
        [HideInInspector] public BoxCollider playerCollider;
        private string currentStateName;
        private FieldOfView fov;

        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public InputAction moveAction;
        [HideInInspector] public InputAction attackAction;

        public GameObject arrowPrefab;
        public GameObject bow;
        [HideInInspector] public GameObject arrow;

        private void Awake()
        {
            moveAction = GetComponent<PlayerInput>().actions["Move"];
            attackAction = GetComponent<PlayerInput>().actions["Attack"];
            animator = GetComponent<Animator>();
            playerCollider = GetComponent<BoxCollider>();
            characterController = GetComponent<CharacterController>();
            renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            fov = GetComponent<FieldOfView>();
            hpBar.Initialize(playerHealth);

            PlayerFSM = new StateMachine<PlayerState>();
            
            var attackState = new AttackState(this, PlayerFSM, needsExitTime: true);
            var takeDamageState = new TakeDamageState(this, PlayerFSM);
            
            PlayerFSM.AddState(PlayerState.Idle, new IdleState(this, PlayerFSM));
            PlayerFSM.AddState(PlayerState.Move, new MoveState(this, PlayerFSM));
            PlayerFSM.AddState(PlayerState.Die, new DieState(this, PlayerFSM));
            PlayerFSM.AddState(PlayerState.Attack, attackState
                .AddAction("OnSendArrow", () => { attackState.OnSendArrow(); })
                .AddAction("OnCheckAttackClicks", () => { attackState.OnCheckAttackClicks(); }));
            PlayerFSM.AddState(PlayerState.TakeDamage, takeDamageState
                .AddAction<int>("OnHit", (int damage) => { takeDamageState.OnHit(damage); }));
            
            PlayerFSM.AddTransitionFromAny(PlayerState.Die, t => playerHealth <= 0);
            PlayerFSM.AddTriggerTransitionFromAny("TakeDamage", PlayerState.TakeDamage);
            PlayerFSM.AddTransition(PlayerState.Idle, PlayerState.Move, t => moveAction.ReadValue<Vector2>().magnitude > 0.1f);
            PlayerFSM.AddTransition(PlayerState.Move, PlayerState.Idle, t => moveAction.ReadValue<Vector2>().magnitude < 0.1f);
            PlayerFSM.AddTransition(PlayerState.Idle, PlayerState.Attack, t => attackAction.triggered && currentTarget != null);
            PlayerFSM.AddTransition(PlayerState.Move, PlayerState.Attack, t => attackAction.triggered && currentTarget != null);
            PlayerFSM.AddTransition(PlayerState.Attack, PlayerState.Idle, t => moveAction.ReadValue<Vector2>().magnitude < 0.1f);
            PlayerFSM.AddTransition(PlayerState.Attack, PlayerState.Move, t => moveAction.ReadValue<Vector2>().magnitude > 0.1f, forceInstantly:true);
            PlayerFSM.AddTransition(PlayerState.TakeDamage, PlayerState.Idle, t => moveAction.ReadValue<Vector2>().magnitude < 0.1f);
            PlayerFSM.AddTransition(PlayerState.TakeDamage, PlayerState.Move, t => moveAction.ReadValue<Vector2>().magnitude > 0.1f);

            PlayerFSM.SetStartState(PlayerState.Idle);
            PlayerFSM.Init();
        }

        private void Update()
        {
            PlayerFSM.OnLogic();
            
            currentStateName = PlayerFSM.GetActiveHierarchyPath().Split('/')[1];
            stateText.SetText(currentStateName);
            currentTarget = fov.targetObject;
        }

        #region IDamageable Functions

        public void TakeDamage(int damage)
        {
            PlayerFSM.Trigger("TakeDamage");
            PlayerFSM.OnAction<int>("OnHit", damage);
        }

        #endregion

        #region State Actions

        public void SendArrow()
        {
            PlayerFSM.OnAction("OnSendArrow");
        }

        public void CheckAttackClicks()
        {
            PlayerFSM.OnAction("OnCheckAttackClicks");
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Collectable_Key"))
            {
                GameManager.instance.CollectedKey();
                other.GetComponent<BoxCollider>().enabled = false;
                GameplayEvents.KeyCollected.Invoke(GameManager.instance.collectedKeyCount,
                    GameManager.instance.totalKeyCount, other.gameObject, this.gameObject);
            }
        }
    }
}