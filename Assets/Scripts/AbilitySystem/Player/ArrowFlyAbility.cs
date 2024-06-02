using Cysharp.Threading.Tasks;
using DG.Tweening;
using FSM;
using Gameplay.Player;
using UnityEngine;

namespace AbilitySystem.Player
{
    [CreateAssetMenu(menuName = "Custom/Abilities/Player/Arrow Fly Ability")]
    public class ArrowFlyAbility : AbilityBase
    {
        public int arrowFlyParticleIndex;
        public GameObject aoePrefab;
        public float secondsBeforeArrowFall;
        
        private Gameplay.Player.Player _player;
        
        public override void OnCreate(GameObject owner)
        {
            _player = owner.GetComponent<Gameplay.Player.Player>();
        }

        public override void Activate(GameObject target, Vector3 position)
        {
            _player.castingAbility = true;
            _player.transform.DOLookAt(position, .25f);
            _player.animator.SetTrigger(AnimationParameters.SkyShot);
            var muzzle = _player.visualEffects.transform.GetChild(arrowFlyParticleIndex);
            muzzle.transform.position = _player.handSlot.transform.position;
            muzzle.gameObject.SetActive(true);


            AoEAttack(position);
        }

        private async void AoEAttack(Vector3 position)
        {
            await UniTask.WaitForSeconds(secondsBeforeArrowFall);

            var vfx = Instantiate(aoePrefab);
            vfx.transform.position = position;
            
            await UniTask.WaitForSeconds(0.1f);
            
            vfx.GetComponent<ParticleSystem>();
        }

        public override void BeginCooldown()
        {
            _player.castingAbility = false;
        }
    }
}
