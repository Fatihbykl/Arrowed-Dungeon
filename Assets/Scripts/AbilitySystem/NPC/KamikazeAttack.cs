using Cysharp.Threading.Tasks;
using DG.Tweening;
using FSM;
using Gameplay.Enemy;
using Gameplay.Interfaces;
using UnityEngine;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Abilities/NPC/Kamikaze Attack")]
    public class KamikazeAttack : AbilityBase
    {
        public float timeBeforeExplode;
        public float explosionRange;
        public GameObject rangeIndicator;
        public ParticleSystem particle;

        private GameObject _indicator;
        private ParticleSystem _particle;
        private Enemy _enemy;
        
        public override void Activate(GameObject owner, GameObject target)
        {
            _enemy = owner.GetComponent<Enemy>();
            _enemy.agentController.speed = 0;
            _enemy.castingAbility = true;
            _enemy.animator.SetBool(AnimationParameters.Attack, true);
            var pos = _enemy.transform.position;

            _indicator = Instantiate(rangeIndicator);
            _indicator.transform.localScale = new Vector3(explosionRange, explosionRange, explosionRange);
            _indicator.transform.position = pos;

            StartExploding();
        }

        private async void StartExploding()
        {
            await UniTask.WaitForSeconds(timeBeforeExplode);
            _particle = Instantiate(particle);
            var pos = _enemy.transform.position;
            pos.y = 1f;
            _particle.transform.position = pos;
            _particle.Play();
            DealDamage();
            DestroyObjects();
        }

        private void DealDamage()
        {
            Collider[] colliders = Physics.OverlapSphere(_enemy.transform.position, explosionRange, _enemy.playerMask);
            if (colliders.Length > 0)
            {
                colliders[0].GetComponent<IDamageable>().TakeDamage(_enemy.enemySettings.enemyBaseDamage, Vector3.zero);
            }
        }

        private void DestroyObjects()
        {
            Destroy(_indicator);
            Destroy(_particle.gameObject, 2f);
            Destroy(_enemy.gameObject.transform.parent.gameObject);
        }
    }
}