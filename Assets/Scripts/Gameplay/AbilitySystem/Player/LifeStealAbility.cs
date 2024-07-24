using UnityEngine;

namespace Gameplay.AbilitySystem.Player
{
    [CreateAssetMenu(menuName = "Custom/Abilities/Player/Life Steal Ability")]
    public class LifeStealAbility : AbilityBase
    {
        public int particleIndex;

        private Gameplay.Player.Player _player;
        
        public override void OnCreate(GameObject owner)
        {
            _player = owner.GetComponent<Gameplay.Player.Player>();
        }

        public override void Activate(GameObject target)
        {
            _player.castingAbility = true;
        }

        public override void BeginCooldown()
        {
            _player.castingAbility = false;
        }
    }
}
