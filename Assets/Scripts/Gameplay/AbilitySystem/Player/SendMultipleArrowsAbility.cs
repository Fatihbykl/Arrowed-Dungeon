using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Managers;
using UnityEngine;

namespace Gameplay.AbilitySystem.Player
{
    [CreateAssetMenu(menuName = "Custom/Abilities/Player/Multiple Arrow Ability")]
    public class SendMultipleArrowsAbility : AbilityBase
    {
        public GameObject arrowPrefab;
        
        private Gameplay.Player.Player _player;
        public override void OnCreate(GameObject owner)
        {
            _player = owner.GetComponent<Gameplay.Player.Player>();
        }

        public override void Activate(GameObject target, Vector3 direction)
        {
            _player.castingAbility = true;

            CinemachineShaker.Instance.ShakeCamera(1f, 0.5f);
            CreateProjectiles(direction);
        }

        private async void CreateProjectiles(Vector3 direction)
        {
            await _player.transform.DORotateQuaternion(Quaternion.LookRotation(direction, Vector3.up), 0.2f);
            
            var angle = -20;
            var angleIncrease = 10;
            for (int i = 0; i < 5; i++)
            {
                Instantiate(arrowPrefab, _player.handSlot.transform.position,
                    _player.transform.rotation * Quaternion.Euler(0, angle, 0));
                angle += angleIncrease;
            }
        }

        public override void BeginCooldown()
        {
            _player.castingAbility = false;
        }
    }
}
