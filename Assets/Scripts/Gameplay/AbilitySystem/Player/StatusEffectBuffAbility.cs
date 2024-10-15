using Gameplay.StatusEffectSystem;
using UnityEngine;

namespace Gameplay.AbilitySystem.Player
{
    [CreateAssetMenu(menuName = "Custom/Abilities/Player/Status Effect Buff Ability")]
    public class StatusEffectBuffAbility : AbilityBase
    {
        public StatusEffectBase statusEffect;
        public int particleIndex;

        private Gameplay.Player.Player _player;
        
        public override void OnCreate(GameObject owner)
        {
            _player = owner.GetComponent<Gameplay.Player.Player>();
            castTime = statusEffect.duration;
        }

        public override void Activate(GameObject target)
        {
            //_player.castingAbility = true;
            // animation
            statusEffect.ApplyStatus(_player.gameObject);
            _player.visualEffects.transform.GetChild(particleIndex).gameObject.SetActive(true);
        }

        public override void BeginCooldown()
        {
            //_player.castingAbility = false;
        }
    }
}
