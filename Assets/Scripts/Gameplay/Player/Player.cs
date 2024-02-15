using System;
using FSM;
using Gameplay.Interfaces;
using Microlight.MicroBar;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class Player : MonoBehaviour, IDamageable
    {
        public PlayerStats stats;
        public MicroBar hpBar;
        private int playerHealth;

        private GameObject currentTarget;

        private BoxCollider playerCollider;
        private FieldOfView fov;
        
        public Animator animator;
        private float lastAttackTime = 0;
        private InputAction attackAction;

        private GameObject arrow;
        public GameObject arrowPrefab;
        public GameObject bow;
        public bool canMove = true;
        public bool attackModeActive = false;

        private void Awake()
        {
            attackAction = GetComponent<PlayerInput>().actions["Attack"];
            animator = GetComponent<Animator>();
            playerCollider = GetComponent<BoxCollider>();
            fov = GetComponent<FieldOfView>();
            playerHealth = stats.baseHealth;
            
            hpBar.Initialize(playerHealth);
        }

        private void Update()
        {
            currentTarget = fov.targetObject;

            if (attackAction.triggered) { ToggleAttackMode(); }

            if (currentTarget != null && attackModeActive)
            {
                transform.LookAt(currentTarget.transform);
                Attack();
            }
        }

        public void TakeDamage(int damage)
        {
            animator.SetTrigger(AnimationParameters.TakeDamage);
            playerHealth -= damage;
            if (playerHealth <= 0)
            {
                Die();
            }
            hpBar.UpdateHealthBar(playerHealth);
        }

        public void Attack()
        {
            if (Time.time - lastAttackTime >= stats.attackCooldown)
            {
                animator.SetTrigger(AnimationParameters.Attack);
                SendArrow();
                lastAttackTime = Time.time;
            }
        }
        
        public void SendArrow()
        {
            var direction = (currentTarget.transform.position - transform.position).normalized;
            var force = direction * 25f;

            arrow = GameObject.Instantiate(arrowPrefab, bow.transform.position,
                Quaternion.LookRotation(direction));
            arrow.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
        
        private void Die()
        {
            // TODO: prevent enemy attack when dead
            
            playerCollider.enabled = false;
            canMove = false;
    
            animator.SetTrigger(AnimationParameters.Die);
            this.enabled = false;
        }

        private void ToggleAttackMode()
        {
            attackModeActive = !attackModeActive;
            animator.SetBool(AnimationParameters.AttackMode, attackModeActive);
        }
    }
    
    [Serializable]
    public class PlayerStats
    {
        public float runningSpeed;
        public float walkingSpeed;
        public int baseHealth;
        public float attackCooldown;
    }
}