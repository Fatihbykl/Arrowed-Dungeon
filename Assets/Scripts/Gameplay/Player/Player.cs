using System;
using System.Linq;
using AbilitySystem;
using FSM;
using Gameplay.Interfaces;
using InventorySystem;
using Managers;
using Microlight.MicroBar;
using StatSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Gameplay.Player
{
    [RequireComponent(typeof(PlayerStats))]
    public class Player : MonoBehaviour, IDamageable
    {
        public MicroBar hpBar;
        public GameObject bow;
        public GameObject arrow;
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
        private PlayerStats _playerStats;
        private float _lastAttackTime;
        private GameObject _arrow;
        private FieldOfView _fov;

        public event Action ReleaseBowString;
        public event Action HoldBowString;
        
        private void Awake()
        {
            _attackAction = GetComponent<PlayerInput>().actions["Attack"];
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _playerStats = GetComponent<PlayerStats>();
            animator = GetComponent<Animator>();
            _fov = GetComponent<FieldOfView>();

            _playerStats.InitHealth();
            hpBar.Initialize(_playerStats.health.BaseValue);
            PrepareAbilities();
        }

        private void Update()
        {
            currentTarget = _fov.targetObject;

            if (_attackAction.triggered) { ToggleAttackMode(); }

            if (currentTarget != null && attackModeActive && !castingAbility)
            {
                Attack();
            }
        }

        public void TakeDamage(int damage)
        {
            animator.SetTrigger(AnimationParameters.TakeDamage);
            _playerStats.health.AddModifier(new StatModifier(-damage, StatModType.Flat));
            CreateDamageText(damage);
            
            if (_playerStats.health.Value <= 0) { Die(); }

            hpBar.UpdateHealthBar(_playerStats.health.Value);
        }
        
        private void CreateDamageText(int damage)
        {
            var textPos = new Vector3(transform.position.x, 2f, transform.position.z);
            textPos.x += (Random.value - 0.5f) / 3f;
            textPos.y += Random.value;
            textPos.z += (Random.value - 0.5f) / 3f;
            DynamicTextManager.CreateText(textPos, damage.ToString(), DynamicTextManager.defaultData);
        }

        public void AttachBow()
        {
            bow.SetActive(true);
            arrow.SetActive(true);
            OnHoldBowString();
        }

        public void DisarmBow()
        {
            bow.SetActive(false);
            arrow.SetActive(false);
            OnReleaseBowString();
        }

        private void Attack()
        {
            if (Time.time - _lastAttackTime >= _playerStats.attackCooldown.Value)
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
            arrow.SetActive(false);
            ReleaseBowString?.Invoke();
        }

        public void OnHoldBowString()
        {
            arrow.SetActive(true);
            HoldBowString?.Invoke();
        }

        private void Die()
        {
            // TODO: prevent enemy attack when dead

            _capsuleCollider.enabled = false;
            
            animator.SetLayerWeight(1, 0);
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
        
        private void PrepareAbilities()
        {
            var abilities = Inventory.Instance.skills;
            if (abilities == null) { return; }
            foreach (var ability in abilities)
            {
                var holder = gameObject.AddComponent<AbilityHolder>();
                holder.ability = Instantiate(ability);
                holder.ability.OnCreate(gameObject);
            }
        }
    }
}