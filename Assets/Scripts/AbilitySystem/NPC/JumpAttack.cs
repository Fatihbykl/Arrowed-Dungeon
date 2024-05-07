using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FSM;
using Gameplay.Enemy;
using Gameplay.Interfaces;
using UnityEngine;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Custom/Abilities/NPC/Jump Attack")]
    public class JumpAttack : AbilityBase
    {
        public GameObject indicator;
        public float damageRange;
        public int abilityDamage;
        public GameObject particle;

        private GameObject _particle;
        private Enemy _enemy;
        private AnimationClip _clip;
        private AnimationEvent _event;
        private float _distanceToTarget;
        private Vector3 _targetPos;
        private GameObject _indicator;
        
        public override void OnCreate(GameObject owner)
        {
            _enemy = owner.GetComponent<Enemy>();
            _enemy.JumpAttackJumpEvent += OnAnimationJump;
            _enemy.JumpAttackLandEvent += OnAnimationLand;
        }

        public override void Activate(GameObject target)
        {
            _targetPos = _enemy.player.transform.position;
            _distanceToTarget = Vector3.Distance(_enemy.transform.position, _targetPos);
            _enemy.transform.DOLookAt(_targetPos, 0.5f);
            
            _indicator = Instantiate(indicator);
            _indicator.transform.position = _targetPos;
            _indicator.transform.localScale = new Vector3(damageRange / 2.4f, damageRange / 2.4f, damageRange / 2.4f);
            
            _enemy.castingAbility = true;
            _enemy.agentController.speed = 1f;
            _enemy.letAIManagerSetDestination = false;
            _enemy.animator.SetTrigger(AnimationParameters.JumpAttack);
        }

        private void OnAnimationJump(GameObject sender)
        {
            if (sender != _enemy.gameObject) { return; }
            
            _enemy.agentController.speed = 3;
            _enemy.agentController.agent.SetDestination(_targetPos);
        }

        private void OnAnimationLand(GameObject sender)
        {
            if (sender != _enemy.gameObject) { return; }
            
            _particle = Instantiate(particle);
            _particle.transform.position = _targetPos;
            _particle.transform.localScale = new Vector3(damageRange / 2.4f, damageRange / 2.4f, damageRange / 2.4f);
            
            DealDamage();
            DestroyObjects();
            _enemy.agentController.speed = 0f;
        }

        private void DealDamage()
        {
            Collider[] colliders = Physics.OverlapSphere(_enemy.transform.position, damageRange, _enemy.playerMask);
            if (colliders.Length > 0)
            {
                colliders[0].GetComponent<IDamageable>().TakeDamage(abilityDamage);
            }
        }

        private void DestroyObjects()
        {
            Destroy(_indicator);
            Destroy(_particle.gameObject, 2f);
        }

        public override void BeginCooldown()
        {
            _enemy.castingAbility = false;
            _enemy.letAIManagerSetDestination = true;
            _enemy.agentController.speed = _enemy.enemyStats.chaseSpeed.Value;
        }

        private void OnDestroy()
        {
            _enemy.JumpAttackJumpEvent -= OnAnimationJump;
            _enemy.JumpAttackLandEvent -= OnAnimationLand;
        }
    }
}
