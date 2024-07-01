using Animations;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gameplay.DamageDealers;
using UnityEngine;

namespace Gameplay.AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Custom/Abilities/NPC/Line Attack")]
    public class LineAttack : AbilityBase
    {
        public ParticleSystem slashCharge;
        public GameObject slashImpact;

        private Enemy.Enemy _enemy;
        private GameObject _indicator;
        private Vector3 _targetDirection;
        private ParticleSystem _slashCharge;
        private BoxCollider _boxCollider;
        private Vector3 _currentPos;

        public override void OnCreate(GameObject owner)
        {
            _enemy = owner.GetComponent<Enemy.Enemy>();
            _enemy.LineAttackHitEvent += OnHitGround;
        }

        public override void Activate(GameObject target)
        {
            _enemy.castingAbility = true;
            _enemy.agentController.speed = 0f;
            _enemy.animator.SetTrigger(AnimationParameters.LineAttack);
            _targetDirection = (_enemy.player.transform.position - _enemy.transform.position).normalized;
            _currentPos = _enemy.transform.position;
            _currentPos.y = 1f;
            _slashCharge = Instantiate(slashCharge, _enemy.GetComponentInChildren<WeaponDamageDealer>().gameObject.transform);

            StartAttack();
        }

        private async void StartAttack()
        {
            await _enemy.transform.DOLookAt(_enemy.player.transform.position, 0.2f);
            await UniTask.WaitForSeconds(0.2f);
            await UniTask.WaitForSeconds(castTime);
        }

        public override void BeginCooldown()
        {
            _enemy.castingAbility = false;
            _enemy.agentController.speed = _enemy.enemyStats.chaseSpeed.Value;
        }

        private void OnHitGround(GameObject sender)
        {
            if (sender != _enemy.gameObject) { return; }
            
            _slashCharge.Stop();
            Instantiate(slashImpact, _currentPos, Quaternion.LookRotation(_targetDirection));
        }

        private void OnDestroy()
        {
            _enemy.LineAttackHitEvent -= OnHitGround;
        }
    }
}