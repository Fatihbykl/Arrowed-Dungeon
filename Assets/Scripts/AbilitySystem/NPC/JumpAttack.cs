using Cysharp.Threading.Tasks;
using DG.Tweening;
using FSM;
using Gameplay.Enemy;
using Gameplay.Interfaces;
using UnityEngine;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Abilities/NPC/Jump Attack")]
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
        private float start, end;
        
        public override void Activate(GameObject owner, GameObject target)
        {
            _enemy = owner.GetComponent<Enemy>();
            
            _targetPos = _enemy.player.transform.position;
            _distanceToTarget = Vector3.Distance(_enemy.transform.position, _targetPos);
            
            _particle = Instantiate(particle);
            _particle.transform.position = _targetPos;
            // _indicator.transform.localScale = new Vector3(damageRange, damageRange, damageRange);
            
            _enemy.castingAbility = true;
            _enemy.agentController.speed = 1f;
            _enemy.letAIManagerSetDestination = false;
            _enemy.animator.SetTrigger(AnimationParameters.JumpAttack);
            start = Time.time;
        }

        public async void OnAnimationJump()
        {
            
            _enemy.agentController.speed = 3;
            _enemy.agentController.agent.SetDestination(_targetPos);
 
            // _particle = Instantiate(particle);
            // var particlePos = _targetPos;
            // particlePos.y = 0.5f;
            // _particle.transform.position = particlePos;
            // _particle.Play();
        }
        
        private void DealDamage()
        {
            Collider[] colliders = Physics.OverlapSphere(_enemy.transform.position, damageRange, _enemy.playerMask);
            if (colliders.Length > 0)
            {
                colliders[0].GetComponent<IDamageable>().TakeDamage(abilityDamage, Vector3.zero);
            }
        }

        private void DestroyObjects()
        {
            Destroy(_indicator);
            Destroy(_particle.gameObject, 2f);
        }

        public void OnAnimationLand()
        {
            DealDamage();
            DestroyObjects();
            end = Time.time;
            Debug.Log(end - start);
            _enemy.agentController.speed = 0f;
        }

        public override void BeginCooldown()
        {
            _enemy.castingAbility = false;
            _enemy.letAIManagerSetDestination = true;
            _enemy.agentController.speed = _enemy.enemySettings.chaseSpeed;
        }
    }
}
