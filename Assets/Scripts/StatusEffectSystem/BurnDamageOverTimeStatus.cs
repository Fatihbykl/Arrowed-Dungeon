using Cysharp.Threading.Tasks;
using Gameplay.Enemy;
using Gameplay.Interfaces;
using UnityEngine;

namespace StatusEffectSystem
{
    [CreateAssetMenu(menuName = "Custom/Status Effect/Enemy/Burn Damage Over Time Status")]
    public class BurnDamageOverTimeStatus : StatusEffectBase
    {
        public float interval;
        public int damageEveryInterval;
        
        private Enemy _enemy;
        private GameObject _particle;
        
        public override void ApplyStatus(GameObject target)
        {
            _enemy = target.GetComponent<Enemy>();
            _particle = Instantiate(vfxPrefab, _enemy.transform);
            _particle.transform.position = Vector3.zero;

            DamageOverTime();
        }

        private async void DamageOverTime()
        {
            var damageCount = duration / interval;
            for (int i = 0; i < damageCount; i++)
            {
                if (!_enemy) { return; }
                _enemy.TakeDamage(damageEveryInterval);
                await UniTask.WaitForSeconds(interval);
            }
            RemoveStatus();
        }

        public override void RemoveStatus()
        {
            if (!_enemy) { return; }
            
            Destroy(_particle);
        }
    }
}