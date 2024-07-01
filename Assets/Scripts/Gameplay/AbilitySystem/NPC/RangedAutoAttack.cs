using Animations;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Custom/Abilities/NPC/Ranged Auto Attack")]
    public class RangedAutoAttack : AbilityBase
    {
        [Header("Projectile Settings")]
        public Projectile projectilePrefab;
        
        private Enemy.Enemy _enemy;
        private Vector3 _targetPosition;
        private Animator _animator;
        private AnimationEvent _animationEvent;
        private AnimationClip _animationClip;

        public override void OnCreate(GameObject owner)
        {
            _enemy = owner.GetComponent<Enemy.Enemy>();
            _enemy.RangedAutoAttackEvent += OnSendProjectile;
        }

        public override void Activate(GameObject target)
        {
            _enemy.castingAbility = true;
            _enemy.agentController.speed = 0f;
            _targetPosition = _enemy.player.transform.position;
            _enemy.transform.DOLookAt(_targetPosition, .4f);
            _targetPosition.y = 1f;
            _enemy.animator.SetTrigger(AnimationParameters.Attack);
        }


        public override void BeginCooldown()
        {
            _enemy.agentController.speed = _enemy.enemyStats.chaseSpeed.Value;
            _enemy.castingAbility = false;
        }

        private void OnSendProjectile(GameObject sender)
        {
            if (sender != _enemy.gameObject) { return; }
            
            var projectile = Instantiate(projectilePrefab);
            projectile.transform.position = _enemy.projectileSpawnPosition.transform.position;
            projectile.transform.LookAt(_targetPosition);
            projectile.target = _enemy.player.gameObject;
        }

        private void OnDestroy()
        {
            _enemy.RangedAutoAttackEvent -= OnSendProjectile;
        }
    }
}
