using Cysharp.Threading.Tasks;
using DG.Tweening;
using FSM;
using Gameplay.Enemy;
using Gameplay.Interfaces;
using UnityEngine;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Custom/Abilities/NPC/Kamikaze Attack")]
    public class KamikazeAttack : AbilityBase
    {
        public float timeBeforeExplode;
        public float explosionRange;
        public GameObject particle;

        private GameObject _particle;
        private Enemy _enemy;

        public override void OnCreate(GameObject owner)
        {
            _enemy = owner.GetComponent<Enemy>();
        }

        public override void Activate(GameObject target)
        {
            _enemy.agentController.speed = 0;
            _enemy.castingAbility = true;
            _enemy.animator.SetBool(AnimationParameters.Attack, true);
            var pos = _enemy.transform.position;

            _particle = Instantiate(particle);
            _particle.transform.localScale = new Vector3(explosionRange / 3, explosionRange / 3, explosionRange / 3);
            _particle.transform.position = pos;

            StartExploding();
        }

        private async void StartExploding()
        {
            await UniTask.WaitForSeconds(timeBeforeExplode);
            _enemy.TriggerDie();
            DealDamage();
            DestroyObjects();
        }

        private void DealDamage()
        {
            Collider[] colliders = Physics.OverlapSphere(_enemy.transform.position, explosionRange, _enemy.playerMask);
            if (colliders.Length > 0)
            {
                colliders[0].GetComponent<IDamageable>().TakeDamage(_enemy.enemyStats.damage.Value);
            }
        }

        private void DestroyObjects()
        {
            Destroy(_particle.gameObject, 2f);
        }
    }
}