using Animations;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FSM;
using Gameplay.Interfaces;
using Gameplay.Managers;
using UnityEngine;

namespace Gameplay.AbilitySystem.Player
{
    [CreateAssetMenu(menuName = "Custom/Abilities/Player/Arrow Fly Ability")]
    public class ArrowFlyAbility : AbilityBase
    {
        public int arrowFlyParticleIndex;
        public float secondsBeforeArrowFall;
        public float damageRadius;
        public GameObject aoePrefab;

        [Header("Sound Effects")]
        public SoundClip flySound;
        public SoundClip arrowHitSound;

        private Gameplay.Player.Player _player;
        private float _circleScale;
        
        public override void OnCreate(GameObject owner)
        {
            _player = owner.GetComponent<Gameplay.Player.Player>();
        }

        public override void Activate(GameObject target, Vector3 position)
        {
            _player.castingAbility = true;
            //_player.transform.DOLookAt(position, .25f);
            _player.animator.SetTrigger(AnimationParameters.SkyShot);
            _circleScale = damageRadius / 4.5f;
            
            var muzzle = _player.visualEffects.transform.GetChild(arrowFlyParticleIndex);
            muzzle.transform.position = _player.handSlot.transform.position;
            muzzle.gameObject.SetActive(true);
            
            AudioManager.Instance.PlaySoundFXClip(flySound, _player.transform);

            position.y = 0.1f;
            AoEAttack(position);
        }

        private async void AoEAttack(Vector3 position)
        {
            await UniTask.WaitForSeconds(secondsBeforeArrowFall);

            var vfx = Instantiate(aoePrefab);
            vfx.transform.localScale = Vector3.one * _circleScale;
            vfx.transform.position = position;
            
            await UniTask.WaitForSeconds(0.1f);
            
            CinemachineShaker.Instance.ShakeCamera(1.5f, 1f);
            AudioManager.Instance.PlaySoundFXClip(arrowHitSound, _player.transform);

            DealDamage(position);
        }

        private void DealDamage(Vector3 position)
        {
            Collider[] colliders = Physics.OverlapSphere(position, damageRadius, 1 << 6);
            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent(out IDamageable player))
                    {
                        player.TakeDamage(50);
                    }
                }
            }
        }

        public override void BeginCooldown()
        {
            _player.castingAbility = false;
        }
    }
}
