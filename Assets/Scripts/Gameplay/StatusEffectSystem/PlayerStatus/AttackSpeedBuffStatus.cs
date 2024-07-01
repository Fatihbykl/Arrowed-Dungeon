using Cysharp.Threading.Tasks;
using Gameplay.Player;
using Gameplay.StatSystem;
using UnityEngine;

namespace Gameplay.StatusEffectSystem.PlayerStatus
{
    [CreateAssetMenu(menuName = "Custom/Status Effect/Player/Attack Speed Buff Status")]
    public class AttackSpeedBuffStatus : StatusEffectBase
    {
        [Range(0, 1f)]
        public float attackSpeedPercentage;

        private Player.Player _player;
        private PlayerStats _playerStats;
        private StatModifier _attackSpeedModifier;
        
        public override void ApplyStatus(GameObject target)
        {
            _player = target.GetComponent<Player.Player>();
            _playerStats = _player.GetComponent<PlayerStats>();

            BuffPlayer();
        }

        private async void BuffPlayer()
        {
            _attackSpeedModifier = new StatModifier(-attackSpeedPercentage, StatModType.PercentAdd);
            _playerStats.attackCooldown.AddModifier(_attackSpeedModifier);

            await UniTask.WaitForSeconds(duration);
            RemoveStatus();
        }

        public override void RemoveStatus()
        {
            _playerStats.attackCooldown.RemoveModifier(_attackSpeedModifier);
        }
    }
}
