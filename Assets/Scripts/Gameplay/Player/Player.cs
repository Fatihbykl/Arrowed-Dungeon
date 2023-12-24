using System;
using FSM.Player;
using FSM.Player.States;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityHFSM;

namespace Gameplay.Player
{
    public class Player : MonoBehaviour
    {
        public TMP_Text stateText;
        public float comboDelay;
        private StateMachine<PlayerState> PlayerFSM;
        [HideInInspector] public Animator animator;
        [HideInInspector] public InputAction moveAction;
        [HideInInspector] public InputAction attackAction;
        private Rigidbody rb;
        private float velocity;
        
        private void Awake()
        {
            moveAction = GetComponent<PlayerInput>().actions["Move"];
            attackAction = GetComponent<PlayerInput>().actions["Attack"];
            animator = gameObject.GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            
            PlayerFSM = new StateMachine<PlayerState>();
            
            PlayerFSM.AddState(PlayerState.Idle, new IdleState(this, PlayerFSM));
            PlayerFSM.AddState(PlayerState.Move, new MoveState(this, PlayerFSM));
            PlayerFSM.AddState(PlayerState.Attack, new AttackState(this, PlayerFSM, needsExitTime: true));
            
            PlayerFSM.AddTransition(PlayerState.Idle, PlayerState.Move, t => moveAction.ReadValue<Vector2>().magnitude > 0.1f);
            PlayerFSM.AddTransition(PlayerState.Move, PlayerState.Idle, t => moveAction.ReadValue<Vector2>().magnitude < 0.1f);
            PlayerFSM.AddTransition(PlayerState.Idle, PlayerState.Attack, t => attackAction.triggered);
            PlayerFSM.AddTransition(PlayerState.Move, PlayerState.Attack, t => attackAction.triggered);
            PlayerFSM.AddTransition(PlayerState.Attack, PlayerState.Idle, t => moveAction.ReadValue<Vector2>().magnitude < 0.1f);
            PlayerFSM.AddTransition(PlayerState.Attack, PlayerState.Move, t => moveAction.ReadValue<Vector2>().magnitude > 0.1f);
            
            PlayerFSM.SetStartState(PlayerState.Idle);
            PlayerFSM.Init();
        }

        private void Update()
        {
            PlayerFSM.OnLogic();
            stateText.SetText(PlayerFSM.GetActiveHierarchyPath().Split('/')[1]);
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