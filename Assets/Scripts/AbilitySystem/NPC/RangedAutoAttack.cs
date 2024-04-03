using DG.Tweening;
using FSM;
using Gameplay;
using Gameplay.Enemy;
using UnityEngine;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Abilities/NPC/Ranged Auto Attack")]
    public class RangedAutoAttack : AbilityBase
    {
        [Header("Projectile Settings")]
        public Projectile projectilePrefab;
        
        private Enemy _enemy;
        private Vector3 _targetPosition;
        
        public override void Activate(GameObject owner, GameObject target)
        {
            _enemy = owner.GetComponent<Enemy>();

            _enemy.castingAbility = true;
            _enemy.agentController.speed = 0f;
            _targetPosition = _enemy.player.transform.position;
            _enemy.transform.DOLookAt(_targetPosition, .4f);
            _enemy.animator.SetTrigger(AnimationParameters.Attack);
        }

        public override void BeginCooldown(GameObject owner, GameObject target)
        {
            _enemy.agentController.speed = _enemy.enemySettings.chaseSpeed;
            _enemy.castingAbility = false;
        }

        public void SendProjectile()
        {
            // var direction = (target.transform.position - owner.transform.position).normalized;
            // var force = direction * 25f;
            //
            // var spawn = new Vector3(owner.transform.position.x, 1, owner.transform.position.z);
            // var arrow = GameObject.Instantiate(projectilePrefab, spawn,
            //     Quaternion.LookRotation(direction));
            // arrow.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

            var projectile = Instantiate(projectilePrefab);
            projectile.transform.position = _enemy.projectileSpawnPosition.transform.position;
            projectile.transform.LookAt(_targetPosition);
            projectile.target = _enemy.player.gameObject;
        }
    }
}
