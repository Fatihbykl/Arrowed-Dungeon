using System;
using DG.Tweening;
using Events;
using FSM;
using Gameplay.Interfaces;
using Managers;
using Microlight.MicroBar;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class Player : MonoBehaviour, IDamageable
    {
        public PlayerStats stats;
        public MicroBar hpBar;
        public GameObject bowPlacement;
        public GameObject handSlot;
        private int playerHealth;

        public GameObject currentTarget;

        private CapsuleCollider capsuleCollider;
        private FieldOfView fov;

        public Animator animator;
        private float lastAttackTime;
        private InputAction attackAction;

        private GameObject arrow;
        public Projectile arrowPrefab;
        public GameObject bow;
        public bool canMove = true;
        public bool attackModeActive = false;

        public ParticleSystem dustParticle;
        
        private void Awake()
        {
            attackAction = GetComponent<PlayerInput>().actions["Attack"];
            animator = GetComponent<Animator>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            fov = GetComponent<FieldOfView>();
            playerHealth = stats.baseHealth;

            hpBar.Initialize(playerHealth);
        }

        private void Update()
        {
            currentTarget = fov.targetObject;

            if (attackAction.triggered)
            {
                ToggleAttackMode();
            }

            if (currentTarget != null && attackModeActive)
            {
                //transform.LookAt(currentTarget.transform);
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

        public void AttachBow()
        {
            bow.transform.SetParent(handSlot.transform, false);
            HoldBowString();
        }

        public void DisarmBow()
        {
            bow.transform.SetParent(bowPlacement.transform, false);
            ReleaseBowString();
        }

        private void Attack()
        {
            if (Time.time - lastAttackTime >= stats.attackCooldown)
            {
                animator.SetTrigger(AnimationParameters.Attack);
                lastAttackTime = Time.time;
            }
        }

        public void SendArrow()
        {
            var targetPos = currentTarget.transform.position;
            targetPos.y = 1f;
            Projectile projectile = Instantiate(arrowPrefab);
            projectile.transform.position = bow.transform.position;
            projectile.transform.LookAt(targetPos);
            projectile.target = currentTarget;
            
            AudioManager.instance.PlayArrowWooshSFX();

            ReleaseBowString();
        }

        public void PlayFootstepSound()
        {
            AudioManager.instance.PlayFootstepSFX();
            dustParticle.Play();
        }

        private void ReleaseBowString()
        {
            GameplayEvents.ReleaseBowString?.Invoke();
        }

        public void HoldBowString()
        {
            GameplayEvents.HoldBowString?.Invoke();
        }

        private void Die()
        {
            // TODO: prevent enemy attack when dead

            capsuleCollider.enabled = false;
            canMove = false;

            animator.SetTrigger(AnimationParameters.Die);
            this.enabled = false;
        }

        private void ToggleAttackMode()
        {
            lastAttackTime = Time.time;
            attackModeActive = !attackModeActive;
            animator.SetTrigger(attackModeActive ? AnimationParameters.EquipBow : AnimationParameters.DisarmBow);
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