using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.AbilitySystem.Player
{
    [CreateAssetMenu(menuName = "Custom/Abilities/Player/Shield Ability")]
    public class ShieldAbility : AbilityBase
    {
        public float duration;
        public int particleIndex;

        private Gameplay.Player.Player _player;
        public override void OnCreate(GameObject owner)
        {
            _player = owner.GetComponent<Gameplay.Player.Player>();
        }

        public override void Activate(GameObject target)
        {
            _player.castingAbility = true;
            
            ActivateForSeconds();
        }

        private async void ActivateForSeconds()
        {
            _player.isInvulnerable = true;
            _player.visualEffects.transform.GetChild(particleIndex).gameObject.SetActive(true);

            await UniTask.WaitForSeconds(duration);
            
            _player.isInvulnerable = false;
            _player.visualEffects.transform.GetChild(particleIndex).gameObject.SetActive(false);
        }

        public override void BeginCooldown()
        {
            _player.castingAbility = false;
        }
    }
}
