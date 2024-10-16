using Animations;
using Cysharp.Threading.Tasks;
using Gameplay.Managers;
using Gameplay.StatSystem;
using UnityEngine;

namespace Gameplay.StatusEffectSystem.EnemyStatus
{
    [CreateAssetMenu(menuName = "Custom/Status Effect/Enemy/Stun by Chance Status")]
    public class StunByChanceStatus : StatusEffectBase
    {
        [Range(0, 1f)]
        public float stunProbability;
        
        private GameObject _particle;
        private StatModifier _modifier;
        
        private Enemy.Enemy _enemy;
        public override void ApplyStatus(GameObject target)
        {
            _enemy = target.GetComponent<Enemy.Enemy>();
            if (_enemy == null) { return; }

            AudioManager.Instance.PlayRandomSoundFXClip(soundClips, _enemy.transform);
            if (!_enemy.isInStatusEffect && Random.Range(0f, 1f) < stunProbability)
            {
                StunEnemy();
            }
        }

        private async void StunEnemy()
        {
            _modifier = new StatModifier(-1, StatModType.PercentAdd);
            _enemy.enemyStats.chaseSpeed.AddModifier(_modifier);
            _enemy.isInStatusEffect = true;
            _enemy.animator.SetBool(AnimationParameters.Stun, true);
            
            _particle = Instantiate(vfxPrefab, _enemy.transform);
            _particle.transform.position = _enemy.transform.position;

            await UniTask.WaitForSeconds(duration);
            RemoveStatus();
        }

        public override void RemoveStatus()
        {
            _enemy.isInStatusEffect = false;
            _enemy.animator.SetBool(AnimationParameters.Stun, false);
            _enemy.enemyStats.chaseSpeed.RemoveModifier(_modifier);
            Destroy(_particle);
        }
    }
}