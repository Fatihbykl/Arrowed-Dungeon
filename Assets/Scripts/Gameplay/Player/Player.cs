using System;
using DG.Tweening;
using Events;
using FSM;
using Gameplay.Interfaces;
using Managers;
using Microlight.MicroBar;
using StatSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Player
{
    public class Player : MonoBehaviour, IDamageable
    {
        public PlayerStats playerStats;
        public MicroBar hpBar;
        public GameObject bow;
        public GameObject bowPlacement;
        public GameObject handSlot;
        public Projectile arrowPrefab;
        public GameObject visualEffects;

        [HideInInspector] public GameObject currentTarget;
        [HideInInspector] public Animator animator;
        [HideInInspector] public bool attackModeActive;
        [HideInInspector] public bool castingAbility;

        private CapsuleCollider _capsuleCollider;
        private InputAction _attackAction;
        private float _lastAttackTime;
        private GameObject _arrow;
        private FieldOfView _fov;

        public event Action ReleaseBowString;
        public event Action HoldBowString;
        
        private void Awake()
        {
            _attackAction = GetComponent<PlayerInput>().actions["Attack"];
            _capsuleCollider = GetComponent<CapsuleCollider>();
            animator = GetComponent<Animator>();
            _fov = GetComponent<FieldOfView>();

            playerStats.InitHealth();
            hpBar.Initialize(playerStats.health.BaseValue);

            for (int i = 0; i < visualEffects.transform.childCount; i++)
            {
                Debug.Log(visualEffects.transform.GetChild(i).name);
            }
        }

        private void Update()
        {
            currentTarget = _fov.targetObject;

            if (_attackAction.triggered) { ToggleAttackMode(); }
            if (currentTarget != null && attackModeActive) { Attack(); }
        }

        public void TakeDamage(int damage)
        {
            animator.SetTrigger(AnimationParameters.TakeDamage);
            playerStats.health.AddModifier(new StatModifier(-damage, StatModType.Flat));
            
            if (playerStats.health.Value <= 0) { Die(); }

            hpBar.UpdateHealthBar(playerStats.health.Value);
        }

        public void AttachBow()
        {
            bow.transform.SetParent(handSlot.transform, false);
            OnHoldBowString();
        }

        public void DisarmBow()
        {
            bow.transform.SetParent(bowPlacement.transform, false);
            OnReleaseBowString();
        }

        private void Attack()
        {
            if (Time.time - _lastAttackTime >= playerStats.attackCooldown.Value)
            {
                animator.SetTrigger(AnimationParameters.Attack);
                _lastAttackTime = Time.time;
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

            OnReleaseBowString();
        }

        private void OnReleaseBowString()
        {
            ReleaseBowString?.Invoke();
        }

        public void OnHoldBowString()
        {
            HoldBowString?.Invoke();
        }

        private void Die()
        {
            // TODO: prevent enemy attack when dead

            _capsuleCollider.enabled = false;
            
            animator.SetTrigger(AnimationParameters.Die);
            this.enabled = false;
        }

        private void ToggleAttackMode()
        {
            _lastAttackTime = Time.time;
            attackModeActive = !attackModeActive;
            animator.SetTrigger(attackModeActive ? AnimationParameters.EquipBow : AnimationParameters.DisarmBow);
            animator.SetBool(AnimationParameters.AttackMode, attackModeActive);
        }
    }

    [Serializable]
    public class PlayerStats
    {
        public IntegerStat maxHealth;
        public IntegerStat damage;
        public IntegerStat armor;
        public FloatStat missChance; // stability
        public FloatStat runningSpeed;
        public FloatStat walkingSpeed;
        public FloatStat attackCooldown;

        [HideInInspector] public VitalStat health;

        public void InitHealth()
        {
            health.BaseValue = maxHealth.BaseValue;
            health.useUpperBound = true;
            health.upperBound = maxHealth.BaseValue;
        }
    }
}