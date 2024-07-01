using Animations;
using Cysharp.Threading.Tasks;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.AbilitySystem.Player
{
    [CreateAssetMenu(menuName = "Custom/Abilities/Player/Dash Ability")]
    public class DashAbility : AbilityBase
    {
        public float dashSpeed;
        
        private Gameplay.Player.Player _player;
        private PlayerMovement _playerMovement;
        private float _timer;
        
        public override void OnCreate(GameObject owner)
        {
            _player = owner.GetComponent<Gameplay.Player.Player>();
            _playerMovement = owner.GetComponent<PlayerMovement>();
        }

        public override void Activate(GameObject target)
        {
            _player.castingAbility = true;
            _player.animator.SetLayerWeight(1, 0);
            _player.animator.SetTrigger(AnimationParameters.Dash);
            //_player.GetComponent<Rigidbody>().AddForce(_playerMovement.moveDirection * dashSpeed, ForceMode.VelocityChange);
            Dash();
        }

        private async void Dash()
        {
            _timer = Time.time;
            while (Time.time - _timer < castTime)
            {
                _player.GetComponent<Rigidbody>().velocity = _playerMovement.moveDirection * dashSpeed;
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            }
        }

        public override void BeginCooldown()
        {
            _player.castingAbility = false;
            _player.animator.SetLayerWeight(1, 1);
        }
    }
}
