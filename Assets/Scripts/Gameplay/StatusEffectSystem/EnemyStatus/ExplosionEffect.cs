using Gameplay.Enemy;
using Gameplay.Interfaces;
using UnityEngine;

namespace StatusEffectSystem.EnemyStatus
{
    [CreateAssetMenu(menuName = "Custom/Status Effect/Enemy/Explosion Effect")]
    public class ExplosionEffect : StatusEffectBase
    {
        public float explosionRange;
        public int explosionDamage;
        public LayerMask enemyMask;
        
        private Enemy _enemy;
        
        public override void ApplyStatus(GameObject target)
        {
            _enemy = target.GetComponent<Enemy>();
            
            Collider[] colliders = Physics.OverlapSphere(_enemy.transform.position, explosionRange, enemyMask);
            if (colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].GetComponent<IDamageable>().TakeDamage(explosionDamage);
                }
            }
        }
    }
}