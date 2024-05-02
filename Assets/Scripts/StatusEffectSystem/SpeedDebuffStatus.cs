using Cysharp.Threading.Tasks;
using Gameplay.Enemy;
using UnityEngine;

namespace StatusEffectSystem
{
    [CreateAssetMenu(menuName = "Custom/Status Effect/Enemy/Speed Debuff Status")]
    public class SpeedDebuffStatus : StatusEffectBase
    {
        [Range(0, 1)]
        public float slowPercent;
        
        private Enemy _enemy;
        private GameObject _particle;
        private float _slowAmount;
        private float _enemyCurrentSpeed;

        public override void ApplyStatus(GameObject target)
        {
            _enemy = target.GetComponent<Enemy>();
            _particle = Instantiate(vfxPrefab, _enemy.transform);
            _particle.transform.position = Vector3.zero;
            _enemyCurrentSpeed = _enemy.enemySettings.chaseSpeed;
            _slowAmount = _enemyCurrentSpeed * slowPercent;

            SlowDownEnemy();
        }

        private async void SlowDownEnemy()
        {
            _enemy.agentController.speed = _enemyCurrentSpeed - _slowAmount;
            await UniTask.WaitForSeconds(duration);
            RemoveStatus();
        }

        public override void RemoveStatus()
        {
            _enemy.agentController.speed = _enemyCurrentSpeed;
            Destroy(_particle);
        }
    }
}