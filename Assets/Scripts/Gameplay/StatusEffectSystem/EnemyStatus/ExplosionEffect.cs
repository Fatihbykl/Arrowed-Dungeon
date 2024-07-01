using Gameplay.Interfaces;
using Gameplay.Managers;
using UnityEngine;

namespace Gameplay.StatusEffectSystem.EnemyStatus
{
    [CreateAssetMenu(menuName = "Custom/Status Effect/Enemy/Explosion Effect")]
    public class ExplosionEffect : StatusEffectBase
    {
        public float explosionRange;
        public int explosionDamage;
        public LayerMask enemyMask;
        
        private Enemy.Enemy _enemy;
        
        public override void ApplyStatus(GameObject target)
        {
            _enemy = target.GetComponent<Enemy.Enemy>();
            
            Collider[] colliders = Physics.OverlapSphere(_enemy.transform.position, explosionRange, enemyMask);
            AudioManager.Instance.PlayRandomSoundFXClip(soundClips, _enemy.transform);
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