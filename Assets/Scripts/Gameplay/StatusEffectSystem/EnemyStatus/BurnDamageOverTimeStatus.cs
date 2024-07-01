using Cysharp.Threading.Tasks;
using Gameplay.Managers;
using UnityEngine;

namespace Gameplay.StatusEffectSystem.EnemyStatus
{
    [CreateAssetMenu(menuName = "Custom/Status Effect/Enemy/Burn Damage Over Time Status")]
    public class BurnDamageOverTimeStatus : StatusEffectBase
    {
        public float interval;
        public int damageEveryInterval;
        
        private Enemy.Enemy _enemy;
        private GameObject _particle;
        
        public override void ApplyStatus(GameObject target)
        {
            _enemy = target.GetComponent<Enemy.Enemy>();
            
            AudioManager.Instance.PlayRandomSoundFXClip(soundClips, _enemy.transform);
            if (!_enemy.isInStatusEffect)
            {
                _enemy.isInStatusEffect = true;
                _particle = Instantiate(vfxPrefab, _enemy.transform);
                _particle.transform.position = _enemy.transform.position;

                DamageOverTime();
            }
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
            
            _enemy.isInStatusEffect = false;
            Destroy(_particle);
        }
    }
}