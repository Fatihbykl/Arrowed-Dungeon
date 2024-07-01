using Cysharp.Threading.Tasks;
using Gameplay.Player;
using Gameplay.StatSystem;
using UnityEngine;

namespace Gameplay.StatusEffectSystem.PlayerStatus
{
    [CreateAssetMenu(menuName = "Custom/Status Effect/Player/Attack Damage Buff Status")]
    public class AttackDamageBuffStatus : StatusEffectBase
    {
        [Range(0, 1f)]
        public float attackDamagePercentage;

        private Player.Player _player;
        private PlayerStats _playerStats;
        private StatModifier _attackDamageModifier;
        
        public override void ApplyStatus(GameObject target)
        {
            _player = target.GetComponent<Player.Player>();
            _playerStats = _player.GetComponent<PlayerStats>();

            BuffPlayer();
        }

        private async void BuffPlayer()
        {
            _attackDamageModifier = new StatModifier(attackDamagePercentage, StatModType.PercentAdd);
            _playerStats.attackCooldown.AddModifier(_attackDamageModifier);

            await UniTask.WaitForSeconds(duration);
            RemoveStatus();
        }

        public override void RemoveStatus()
        {
            _playerStats.attackCooldown.RemoveModifier(_attackDamageModifier);
        }
    }
}
