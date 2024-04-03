using Cysharp.Threading.Tasks;
using DG.Tweening;
using FSM;
using Gameplay.Enemy;
using Gameplay.Interfaces;
using UnityEngine;
using VFX.AbilityIndicatorScripts;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Abilities/NPC/Line Attack")]
    public class LineAttack : AbilityBase
    {
        public float attackDistance;
        public GameObject indicator;
        public ParticleSystem particle;

        private Enemy _enemy;
        private GameObject _indicator;
        private Vector3 _targetDirection;
        private IndicatorFill _fill;
        private ParticleSystem _particle;
        private BoxCollider _boxCollider;

        public override void Activate(GameObject owner, GameObject target)
        {
            _enemy = owner.GetComponent<Enemy>();
            
            _enemy.castingAbility = true;
            _enemy.agentController.speed = 0f;
            _enemy.animator.SetTrigger(AnimationParameters.LineAttack);
            _targetDirection = (_enemy.player.transform.position - _enemy.transform.position).normalized;

            StartAttack();
        }

        private async void StartAttack()
        {
            _enemy.transform.DOLookAt(_enemy.player.transform.position, 0.2f);
            await UniTask.WaitForSeconds(0.2f);

            CreateIndicator();
            
            DOTween.To(() => _fill.fillProgress, x => _fill.fillProgress = x, 1f, castTime);
            await UniTask.WaitForSeconds(castTime);
        }

        public override void BeginCooldown(GameObject owner, GameObject target)
        {
            _enemy.castingAbility = false;
            _enemy.agentController.speed = _enemy.enemySettings.chaseSpeed;
        }

        public void OnHitGround()
        {
            Vector3 worldCenter = _boxCollider.transform.TransformPoint(_boxCollider.center);
            Vector3 worldHalfExtents = Vector3.Scale(_boxCollider.size, _boxCollider.transform.lossyScale) * 0.5f;
            
            Collider[] colliders = Physics.OverlapBox(worldCenter, worldHalfExtents, _boxCollider.transform.rotation, 1 << 7);
            if (colliders.Length > 0)
            {
                colliders[0].GetComponent<IDamageable>().TakeDamage(_enemy.enemySettings.enemyBaseDamage, Vector3.zero);
            }
            DestroyObjects();
        }

        private void DestroyObjects()
        {
            Destroy(_indicator);
            //Destroy(_particle.gameObject, 2f);
        }

        private void CreateIndicator()
        {
            _indicator = Instantiate(indicator);
            _indicator.transform.position = _enemy.transform.position;
            _indicator.transform.rotation = Quaternion.LookRotation(_enemy.transform.forward);
            _indicator.transform.localScale = new Vector3(1, attackDistance, 1);
            _fill = _indicator.GetComponentInChildren<IndicatorFill>();
            _boxCollider = _indicator.GetComponent<BoxCollider>();
        }
    }
}