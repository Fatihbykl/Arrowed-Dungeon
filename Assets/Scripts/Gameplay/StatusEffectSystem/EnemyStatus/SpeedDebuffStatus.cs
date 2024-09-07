using Cysharp.Threading.Tasks;
using Gameplay.Managers;
using Gameplay.StatSystem;
using UnityEngine;

namespace Gameplay.StatusEffectSystem.EnemyStatus
{
    [CreateAssetMenu(menuName = "Custom/Status Effect/Enemy/Speed Debuff Status")]
    public class SpeedDebuffStatus : StatusEffectBase
    {
        [Range(0, 1)]
        public float slowPercent;
        
        private Enemy.Enemy _enemy;
        private GameObject _particle;
        private StatModifier _modifier;

        public override void ApplyStatus(GameObject target)
        {
            _enemy = target.GetComponent<Enemy.Enemy>();
            if (_enemy == null) { return; }
            
            AudioManager.Instance.PlayRandomSoundFXClip(soundClips, _enemy.transform);
            if (!_enemy.isInStatusEffect)
            {
                _enemy.isInStatusEffect = true;
                _particle = Instantiate(vfxPrefab, _enemy.transform);
                _particle.transform.position = _enemy.transform.position;
                
                SlowDownEnemy();
            }
        }

        private async void SlowDownEnemy()
        {
            _modifier = new StatModifier(-slowPercent, StatModType.PercentAdd);
            _enemy.enemyStats.chaseSpeed.AddModifier(_modifier);
            await UniTask.WaitForSeconds(duration);
            RemoveStatus();
        }

        public override void RemoveStatus()
        {
            _enemy.enemyStats.chaseSpeed.RemoveModifier(_modifier);
            _enemy.isInStatusEffect = false;
            Destroy(_particle);
        }
    }
}