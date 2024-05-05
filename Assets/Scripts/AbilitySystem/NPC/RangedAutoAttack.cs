using System;
using DG.Tweening;
using FSM;
using Gameplay;
using Gameplay.Enemy;
using UnityEngine;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Custom/Abilities/NPC/Ranged Auto Attack")]
    public class RangedAutoAttack : AbilityBase
    {
        [Header("Projectile Settings")]
        public Projectile projectilePrefab;
        
        private Enemy _enemy;
        private Vector3 _targetPosition;
        private Animator _animator;
        private AnimationEvent _animationEvent;
        private AnimationClip _animationClip;

        private void Awake()
        {
            Enemy.RangedAutoAttackEvent += OnSendProjectile;
        }

        public override void Activate(GameObject owner, GameObject target)
        {
            _enemy = owner.GetComponent<Enemy>();

            _enemy.castingAbility = true;
            _enemy.agentController.speed = 0f;
            _targetPosition = _enemy.player.transform.position;
            _targetPosition.y = 1f;
            _enemy.transform.DOLookAt(_targetPosition, .4f);
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
    }
}
