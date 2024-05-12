using Cysharp.Threading.Tasks;
using Gameplay.Player;
using StatSystem;
using UnityEngine;

namespace StatusEffectSystem.PlayerStatus
{
    [CreateAssetMenu(menuName = "Custom/Status Effect/Player/Movement Speed Buff Status")]
    public class MovementSpeedBuffStatus : StatusEffectBase
    {
        [Range(0, 1f)]
        public float speedBuffPercentage;

        private Player _player;
        private StatModifier _speedModifier;
        
        public override void ApplyStatus(GameObject target)
        {
            _player = target.GetComponent<Player>();

            BuffPlayer();
        }

        private async void BuffPlayer()
        {
            _speedModifier = new StatModifier(speedBuffPercentage, StatModType.PercentAdd);
            _player.playerStats.runningSpeed.AddModifier(_speedModifier);
            _player.playerStats.walkingSpeed.AddModifier(_speedModifier);

            await UniTask.WaitForSeconds(duration);
            RemoveStatus();
        }

        public override void RemoveStatus()
        {
            _player.playerStats.runningSpeed.RemoveModifier(_speedModifier);
            _player.playerStats.walkingSpeed.RemoveModifier(_speedModifier);
        }
    }
}
