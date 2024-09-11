using Animations;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.Movement;
using Gameplay.Movement.GroundDetection;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.AbilitySystem.Player
{
    [CreateAssetMenu(menuName = "Custom/Abilities/Player/Dash Ability")]
    public class DashAbility : AbilityBase
    {
        public float dashDistance;
        public GameObject dashStartParticle;
        public GameObject dashEndParticle;
        
        private Gameplay.Player.Player _player;
        private PlayerMovement _playerMovement;
        private GameObject _dashStartParticle;
        private GameObject _dashEndParticle;
        private SkinnedMeshRenderer[] _meshRenderers;
        
        public override void OnCreate(GameObject owner)
        {
            _player = owner.GetComponent<Gameplay.Player.Player>();
            _playerMovement = owner.GetComponent<PlayerMovement>();
            _meshRenderers = owner.GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        public override void Activate(GameObject target)
        {
            _player.castingAbility = true;
            _player.animator.SetLayerWeight(1, 0);
            
            BlinkDash();
        }

        private async void BlinkDash()
        {
            RaycastHit obstacle;
            var startPos = _player.transform.position;
            var destination = startPos + _playerMovement.moveDirection * dashDistance;

            startPos.y = 0.1f;
            destination.y = 0.1f;

            if (Physics.Linecast(startPos, destination, out obstacle))
            {
                destination = startPos + _playerMovement.moveDirection * (obstacle.distance - 0.5f);
            }
            
            _dashStartParticle = Instantiate(dashStartParticle, startPos, Quaternion.identity);
            _dashEndParticle = Instantiate(dashEndParticle, destination, Quaternion.identity);
            
            EmissionAnimation();
            
            await UniTask.WaitForFixedUpdate();

            _player.transform.position = destination;
            
            Destroy(_dashStartParticle, 1f);
            Destroy(_dashEndParticle, 1f);
        }

        private void EmissionAnimation()
        {
            foreach (var meshRenderer in _meshRenderers)
            {
                meshRenderer.material.DOColor(new Color(88 / 255f, 184 / 255f, 1f, 1f) * 500f, 2f);
                meshRenderer.material.DOColor(Color.white, 2f);
            }
        }

        public override void BeginCooldown()
        {
            _player.castingAbility = false;
            _player.animator.SetLayerWeight(1, 1);
        }
    }
}
