using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FSM;
using FSM.Player;
using FSM.Player.States;
using Gameplay.Player.DamageDealers;
using Microlight.MicroBar;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using UnityHFSM;

namespace Gameplay.Player
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private int playerHealth;
        [SerializeField] private MicroBar hpBar;
        [SerializeField] private Material animMaterial;
        public TMP_Text stateText;
        
        private StateMachine<PlayerState> PlayerFSM;
        private SkinnedMeshRenderer[] renderers;
        private BoxCollider playerCollider;
        private string currentStateName;
        
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public InputAction moveAction;
        [HideInInspector] public InputAction attackAction;
        
        private void Awake()
        {
            moveAction = GetComponent<PlayerInput>().actions["Move"];
            attackAction = GetComponent<PlayerInput>().actions["Attack"];
            animator = GetComponent<Animator>();
            playerCollider = GetComponent<BoxCollider>();
            characterController = GetComponent<CharacterController>();
            renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            hpBar.Initialize(playerHealth);
            
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
            currentStateName = PlayerFSM.GetActiveHierarchyPath().Split('/')[1];
            stateText.SetText(currentStateName);
            Debug.DrawRay(transform.position, transform.forward, Color.green);
        }

        public void AttackTransition(int transitionNumber)
        {
            GameplayEvents.AttackTransition.Invoke(transitionNumber);
        }

        public void StartDealDamage()
        {
            GameplayEvents.StartDealDamage.Invoke(this.gameObject);
        }
        
        public void EndDealDamage()
        {
            GameplayEvents.EndDealDamage.Invoke(this.gameObject);
        }

        public void TakeDamage(int damage)
        {
            StartTakeDamageAnim();
            CheckHealth(damage);
            hpBar.UpdateHealthBar(playerHealth);
        }
        
        private void CheckHealth(int damage)
        {
            playerHealth -= damage;
            if (playerHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            playerCollider.enabled = false;
            characterController.enabled = false;
            moveAction.Disable();
            
            animator.SetTrigger(AnimationParameters.Die);
        }

        public void StartTakeDamageAnim()
        {
            animator.SetTrigger(AnimationParameters.TakeDamage);
            // change player color for red transition
            foreach (var r in renderers)
            {
                var currentColor = r.material.color;
                r.material.DOColor(animMaterial.color, .5f).From().SetEase(Ease.InFlash);
                r.material.DOColor(currentColor, .5f);
            }
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