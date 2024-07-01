using Animations;
using DG.Tweening;
using Gameplay.Interfaces;
using Gameplay.Managers;
using Gameplay.Movement.Common;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Custom/Abilities/NPC/Jump Attack")]
    public class JumpAttack : AbilityBase
    {
        public GameObject indicator;
        public float damageRadius;
        public int abilityDamage;
        public GameObject particle;

        [Header("Sound Effects")] [HorizontalLine(color: EColor.White, height: 1f)] [Space(5)]
        public SoundClip smashSoundEffect;

        private GameObject _particle;
        private Enemy.Enemy _enemy;
        private AnimationClip _clip;
        private AnimationEvent _event;
        private Vector3 _targetPos;
        private Vector3 _targetVelocity;
        private GameObject _indicator;
        private float _circleScale;
        
        public override void OnCreate(GameObject owner)
        {
            _enemy = owner.GetComponent<Enemy.Enemy>();
            _enemy.JumpAttackJumpEvent += OnAnimationJump;
            _enemy.JumpAttackLandEvent += OnAnimationLand;
        }

        public override void Activate(GameObject target)
        {
            _targetPos = _enemy.player.transform.position;
            _targetVelocity = _enemy.player.GetComponent<Rigidbody>().velocity.onlyXZ();
            _enemy.transform.DOLookAt(_targetPos, 0.5f);
            _circleScale = damageRadius / 3.5f;
            _targetPos += _targetVelocity * 1.5f;
            
            _indicator = Instantiate(indicator);
            _indicator.transform.position = _targetPos;
            _indicator.transform.localScale = Vector3.one * _circleScale;
            
            _enemy.castingAbility = true;
            _enemy.agentController.speed = 1f;
            _enemy.letAIManagerSetDestination = false;
            _enemy.animator.SetTrigger(AnimationParameters.JumpAttack);
        }

        private void OnAnimationJump(GameObject sender)
        {
            if (sender != _enemy.gameObject) { return; }
            
            _enemy.agentController.speed = 3;
            _enemy.agentController.agent.SetDestination(_targetPos);
        }

        private void OnAnimationLand(GameObject sender)
        {
            if (sender != _enemy.gameObject) { return; }
            
            CinemachineShaker.Instance.ShakeCamera(1.5f, 1f);
            AudioManager.Instance.PlaySoundFXClip(smashSoundEffect, _enemy.transform);
            
            _particle = Instantiate(particle);
            _particle.transform.position = _targetPos;
            _particle.transform.localScale = Vector3.one * _circleScale;
            
            DealDamage();
            DestroyObjects();
            _enemy.agentController.speed = 0f;
        }

        private void DealDamage()
        {
            Collider[] colliders = Physics.OverlapSphere(_enemy.transform.position, damageRadius, _enemy.playerMask);
            if (colliders.Length > 0)
            {
                colliders[0].GetComponent<IDamageable>().TakeDamage(abilityDamage);
            }
        }

        private void DestroyObjects()
        {
            Destroy(_indicator);
            Destroy(_particle.gameObject, 2f);
        }

        public override void BeginCooldown()
        {
            _enemy.castingAbility = false;
            _enemy.letAIManagerSetDestination = true;
            _enemy.agentController.speed = _enemy.enemyStats.chaseSpeed.Value;
        }

        private void OnDestroy()
        {
            _enemy.JumpAttackJumpEvent -= OnAnimationJump;
            _enemy.JumpAttackLandEvent -= OnAnimationLand;
        }
    }
}
