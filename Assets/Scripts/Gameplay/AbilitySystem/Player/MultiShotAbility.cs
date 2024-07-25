using Animations;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Managers;
using UnityEngine;

namespace Gameplay.AbilitySystem.Player
{
    [CreateAssetMenu(menuName = "Custom/Abilities/Player/Multi Shot Ability")]
    public class MultiShotAbility : AbilityBase
    {
        public int castingParticleIndex;
        public Transform particlePosition;
        public GameObject projectilePrefab;
        public float detectRadius;

        private Gameplay.Player.Player _player;
        private GameObject _castingParticle;
        private SkinnedMeshRenderer[] _meshRenderers;
        
        public override void OnCreate(GameObject owner)
        {
            _player = owner.GetComponent<Gameplay.Player.Player>();
            _meshRenderers = owner.GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        public override void Activate(GameObject target)
        {
            _player.castingAbility = true;
            _castingParticle = _player.visualEffects.transform.GetChild(castingParticleIndex).gameObject;
            
            StartCasting();
        }

        private async void StartCasting()
        {
            _player.animator.SetTrigger(AnimationParameters.MultiShotStart);
            await UniTask.WaitForSeconds(1f); // for animation fit

            _castingParticle.SetActive(true);

            await UniTask.WaitForSeconds(castTime - 1f);
            
            SendProjectiles();
            CinemachineShaker.Instance.ShakeCamera(2f, 0.5f);
            _castingParticle.SetActive(false);
            _player.animator.SetTrigger(AnimationParameters.MultiShotEnd);
        }
        
        private void SendProjectiles()
        {
            Collider[] colliders = Physics.OverlapSphere(_player.transform.position, detectRadius, 1 << 6);
            if (colliders.Length > 0)
            {
                foreach (var collider in colliders)
                {
                    Projectile projectile = Instantiate(projectilePrefab).GetComponent<Projectile>();
                    projectile.transform.position = _castingParticle.transform.position;
                    projectile.transform.LookAt(collider.gameObject.transform);
                    projectile.target = collider.gameObject;
                }
            }
        }
        
        private void EmissionAnimation()
        {
            foreach (var meshRenderer in _meshRenderers)
            {
                meshRenderer.material.DOColor(new Color(78 / 255f, 1f, 150 / 255f, 1f) * 500f, 2.25f);
                meshRenderer.material.DOColor(Color.white, 2f);
            }
        }

        public override void BeginCooldown()
        {
            _player.castingAbility = false;
            Debug.Log("bitti");
        }
    }
}
