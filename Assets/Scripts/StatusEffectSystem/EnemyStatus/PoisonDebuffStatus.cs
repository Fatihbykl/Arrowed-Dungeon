using Cysharp.Threading.Tasks;
using Gameplay.Enemy;
using StatSystem;
using UnityEngine;

namespace StatusEffectSystem.EnemyStatus
{
    [CreateAssetMenu(menuName = "Custom/Status Effect/Enemy/Poison Debuff Status")]
    public class PoisonDebuffStatus : StatusEffectBase
    {
        [Range(0, 1f)]
        public float armorDebuffPercentage;
        [Range(0, 1f)]
        public int attackDamageDebuff;
        
        private Enemy _enemy;
        private GameObject _particle;
        private StatModifier _armorModifier;
        private StatModifier _damageModifier;
        
        public override void ApplyStatus(GameObject target)
        {
            _enemy = target.GetComponent<Enemy>();
            if (!_enemy.isInStatusEffect)
            {
                _enemy.isInStatusEffect = true;
                _particle = Instantiate(vfxPrefab, _enemy.transform);
                _particle.transform.position = _enemy.transform.position;

                DebuffEnemy();
            }
        }

        private async void DebuffEnemy()
        {
            _armorModifier = new StatModifier(-armorDebuffPercentage, StatModType.PercentAdd);
            _damageModifier = new StatModifier(-attackDamageDebuff, StatModType.PercentAdd);
            _enemy.enemyStats.armor.AddModifier(_armorModifier);
            _enemy.enemyStats.damage.AddModifier(_damageModifier);

            await UniTask.WaitForSeconds(duration);
            RemoveStatus();
        }

        public override void RemoveStatus()
        {
            _enemy.isInStatusEffect = false;
            _enemy.enemyStats.armor.RemoveModifier(_armorModifier);
            _enemy.enemyStats.armor.RemoveModifier(_damageModifier);
            Destroy(_particle);
        }
    }
}