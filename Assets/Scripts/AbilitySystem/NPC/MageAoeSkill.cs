using Cysharp.Threading.Tasks;
using FSM;
using Gameplay.Enemy;
using Gameplay.Interfaces;
using UnityEngine;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Custom/Abilities/NPC/Mage AoE")]
    public class MageAoeSkill : AbilityBase
    {
        public GameObject particle;
        public int damage;
        public float circleSize;
        public float waitBeforeExplosion;
        
        private Enemy _enemy;
        private float _circleSize;
        private GameObject _particle;
        private Vector3 _explosionPosition;
        
        public override void Activate(GameObject owner, GameObject target)
        {
            _enemy = owner.GetComponent<Enemy>();

            _enemy.castingAbility = true;
            _enemy.agentController.speed = 0f;
            _circleSize = circleSize / 7;
            _enemy.animator.SetTrigger(AnimationParameters.MageAoESkill);

            CreateParticle();
            DealDamageArea();
        }

        private void CreateParticle()
        {
            _explosionPosition = _enemy.player.transform.position;
            
            _particle = Instantiate(particle);
            _particle.transform.localScale = new Vector3(_circleSize, _circleSize, _circleSize);
            _particle.transform.position = _explosionPosition;
        }

        private async void DealDamageArea()
        {
            await UniTask.WaitForSeconds(waitBeforeExplosion);
            
            Collider[] colliders = Physics.OverlapSphere(_explosionPosition, circleSize, _enemy.playerMask);
            if (colliders.Length > 0)
            {
                if (colliders[0].TryGetComponent(out IDamageable player))
                {
                    player.TakeDamage(damage);
                }
            }
            Destroy(_particle, 1f);
        }

        public override void BeginCooldown()
        {
            _enemy.castingAbility = false;
        }
    }
}
