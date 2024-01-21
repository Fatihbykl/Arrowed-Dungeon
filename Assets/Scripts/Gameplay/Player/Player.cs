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

        [HideInInspector] public GameObject currentTarget;
        public TMP_Text stateText;

        private StateMachine<PlayerState> PlayerFSM;
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
            fov = GetComponent<FieldOfView>();
            hpBar.Initialize(playerHealth);

            PlayerFSM = new StateMachine<PlayerState>();
            
            var attackState = new AttackState(this, PlayerFSM, needsExitTime: true);
            var takeDamageState = new TakeDamageState(this, PlayerFSM);
            
            PlayerFSM.AddState(PlayerState.Idle, new IdleState(this, PlayerFSM));
            PlayerFSM.AddState(PlayerState.Move, new MoveState(this, PlayerFSM));
            PlayerFSM.AddState(PlayerState.Die, new DieState(this, PlayerFSM));
            PlayerFSM.AddState(PlayerState.Attack, attackState
                .AddAction("OnSendArrow", () => { attackState.OnSendArrow(); }));
            PlayerFSM.AddState(PlayerState.TakeDamage, takeDamageState
                .AddAction<int>("OnHit", (int damage) => { takeDamageState.OnHit(damage); }));
            
            PlayerFSM.AddTransitionFromAny(PlayerState.Die, t => playerHealth <= 0);
            PlayerFSM.AddTriggerTransitionFromAny("TakeDamage", PlayerState.TakeDamage);
            PlayerFSM.AddTransition(PlayerState.Idle, PlayerState.Move, t => moveAction.ReadValue<Vector2>().magnitude > 0.1f);
            PlayerFSM.AddTransition(PlayerState.Move, PlayerState.Idle, t => moveAction.ReadValue<Vector2>().magnitude < 0.1f);
            PlayerFSM.AddTransition(PlayerState.Idle, PlayerState.Attack, t => attackAction.triggered && currentTarget != null);
            PlayerFSM.AddTransition(PlayerState.Move, PlayerState.Attack, t => attackAction.triggered && currentTarget != null);
            PlayerFSM.AddTransition(PlayerState.Attack, PlayerState.Idle, t => moveAction.ReadValue<Vector2>().magnitude < 0.1f);
            PlayerFSM.AddTransition(PlayerState.Attack, PlayerState.Move, t => moveAction.ReadValue<Vector2>().magnitude > 0.1f);
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

        public void TakeDamage(int damage)
        {
            PlayerFSM.Trigger("TakeDamage");
            PlayerFSM.OnAction<int>("OnHit", damage);
        }
        
        public void SendArrow()
        {
            PlayerFSM.OnAction("OnSendArrow");
        }

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