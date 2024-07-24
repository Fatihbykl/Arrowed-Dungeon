using Cysharp.Threading.Tasks;
using Gameplay.Interfaces;
using UnityEngine;

namespace Gameplay.AbilitySystem.Player
{
    [CreateAssetMenu(menuName = "Custom/Abilities/Player/Heal Circle")]
    public class HealAbility : AbilityBase
    {
        public GameObject circleParticle;
        public int healParticleIndex;
        public LayerMask playerMask;
        public float circleRadius;
        public float healInterval;
        public float healDuration;
        public int healAmountEveryInterval;

        private Enemy.Enemy _enemy;
        private Vector3 _healCirclePosition;
        private GameObject _circleParticle;
        private GameObject _healParticle;
        private float _circleScale;

        private Gameplay.Player.Player _player;
        
        public override void OnCreate(GameObject owner)
        {
            _player = owner.GetComponent<Gameplay.Player.Player>();
        }

        public override void Activate(GameObject target)
        {
            _player.castingAbility = true;
            _circleScale = circleRadius / 4; // adjustment for match visual effect with overlap sphere

            CreateHealCircle();
            HealInterval();
        }
        
        private void CreateHealCircle()
        {
            _healCirclePosition = _player.transform.position;
            
            _circleParticle = Instantiate(circleParticle);
            _circleParticle.transform.localScale = Vector3.one * _circleScale;
            _circleParticle.transform.position = _healCirclePosition;
            _circleParticle.SetActive(true);
        }

        private async void HealInterval()
        {
            _healParticle = _player.visualEffects.transform.GetChild(healParticleIndex).gameObject;
            
            for (int i = 0; i < healDuration / healInterval; i++)
            {
                Collider[] colliders = Physics.OverlapSphere(_healCirclePosition, circleRadius, playerMask);
                if (colliders.Length > 0)
                {
                    for (int j = 0; j < colliders.Length; j++)
                    {
                        if (colliders[j].TryGetComponent(out IHealable player))
                        {
                            player.Heal(healAmountEveryInterval);
                            _healParticle.SetActive(true);
                        }
                    }
                }
                await UniTask.WaitForSeconds(healInterval);
                _healParticle.SetActive(false);
            }

            Destroy(_circleParticle);
        }

        public override void BeginCooldown()
        {
            _player.castingAbility = false;
        }
    }
}
