using StatusEffectSystem;
using UnityEngine;

namespace AbilitySystem.Player
{
    [CreateAssetMenu(menuName = "Custom/Abilities/Player/Movement Speed Buff Ability")]
    public class MovementSpeedBuffAbility : AbilityBase
    {
        public StatusEffectBase speedBuffStatus;
        public int particleIndex;

        private Gameplay.Player.Player _player;
        
        public override void OnCreate(GameObject owner)
        {
            _player = owner.GetComponent<Gameplay.Player.Player>();
        }

        public override void Activate(GameObject target)
        {
            _player.castingAbility = true;
            // animation
            speedBuffStatus.ApplyStatus(_player.gameObject);
            _player.visualEffects.transform.GetChild(particleIndex).gameObject.SetActive(true);
        }

        public override void BeginCooldown()
        {
            _player.castingAbility = false;
        }
    }
}
