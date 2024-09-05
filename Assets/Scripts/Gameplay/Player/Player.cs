using System;
using Animations;
using Events;
using FSM;
using Gameplay.AbilitySystem;
using Gameplay.Interfaces;
using Gameplay.InventorySystem;
using Gameplay.Managers;
using Gameplay.StatSystem;
using UI.Dynamic_Floating_Text.Scripts;
using UI.MicroBar;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Gameplay.Player
{
    [RequireComponent(typeof(PlayerStats))]
    public class Player : MonoBehaviour, IDamageable, IHealable
    {
        public MicroBar hpBar;
        public GameObject arrow;
        public GameObject handSlot;
        public Projectile arrowPrefab;
        public GameObject visualEffects;

        [Header("Sound Effects")]
        public SoundClip[] footstepSounds;


        [HideInInspector] public GameObject currentTarget;
        [HideInInspector] public Animator animator;
        [HideInInspector] public bool attackModeActive;
        [HideInInspector] public bool castingAbility;
        [HideInInspector] public bool isInvulnerable;

        private CapsuleCollider _capsuleCollider;
        private InputAction _attackAction;
        private AudioSource _audioSource;
        private PlayerStats _playerStats;
        private float _lastAttackTime;
        private GameObject _arrow;
        private FieldOfView _fov;
        private GameObject _bow;
        private int _bowIndex = 3;

        public event Action ReleaseBowString;
        public event Action HoldBowString;
        
        private void Awake()
        {
            _attackAction = GetComponent<PlayerInput>().actions["Attack"];
            _bow = handSlot.transform.GetChild(_bowIndex).gameObject;
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _playerStats = GetComponent<PlayerStats>();
            _audioSource = GetComponent<AudioSource>();
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
            if (isInvulnerable)
            {
                CreateFloatingText("INVULNERABLE!", DynamicTextManager.Instance.PlayerDamageData);
                return;
            }
            
            damage = Mathf.RoundToInt(damage * (1 - _playerStats.armor.Value / 100f));
            
            animator.SetTrigger(AnimationParameters.TakeDamage);
            _playerStats.health.AddModifier(new StatModifier(-damage, StatModType.Flat));
            CreateFloatingText(damage.ToString(), DynamicTextManager.Instance.PlayerDamageData);
            
            if (_playerStats.health.Value <= 0) { Die(); }

            hpBar.UpdateHealthBar(_playerStats.health.Value);
        }
        
        private void CreateFloatingText(string damage, DynamicTextData data)
        {
            DynamicTextManager.Instance.CreateText(transform, damage, data);
        }

        public void AttachBow()
        {
            _bow.SetActive(true);
            arrow.SetActive(true);
            OnHoldBowString();
        }

        public void DisarmBow()
        {
            _bow.SetActive(false);
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
            projectile.transform.position = _bow.transform.position;
            projectile.transform.LookAt(targetPos);
            projectile.target = currentTarget;

            projectile.damage = Random.Range(0, 1000) <= _playerStats.missChance.Value * 10 ? 0 : _playerStats.damage.Value;
            
            CinemachineShaker.Instance.ShakeCamera(1f, 0.5f);

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

        public void Footsteps()
        {
            AudioManager.Instance.PlayRandomSoundFXWithSource(_audioSource, footstepSounds);
        }

        private void Die()
        {
            // TODO: prevent enemy attack when dead

            _capsuleCollider.enabled = false;
            
            animator.SetLayerWeight(1, 0);
            animator.SetTrigger(AnimationParameters.Die);
            this.enabled = false;
            
            EventManager.EmitEvent(EventStrings.LevelLost, 2.5f);
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

        public void Heal(int healthAmount)
        {
            CreateFloatingText(healthAmount.ToString(), DynamicTextManager.Instance.PlayerHealData);
            _playerStats.health.AddModifier(new StatModifier(healthAmount, StatModType.Flat));
        }
    }
}