using System.Collections.Generic;
using System.Net;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FSM;
using Gameplay.Enemy;
using UnityEngine;
using VFX.AbilityIndicatorScripts;

namespace AbilitySystem.NPC
{
    [CreateAssetMenu(menuName = "Abilities/NPC/Ranged Throw Attack")]
    public class RangedThrowAttack : AbilityBase
    {
        [SerializeField] private GameObject throwableObjectPrefab;
        [SerializeField] private float flyTime;
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private GameObject indicatorPrefab;

        private Vector3 startPoint;
        private Vector3 endPoint;
        private IndicatorFill _fill;
        private Enemy enemy;
        private GameObject indicator;
        private GameObject throwableObject;
        
        public override void Activate(GameObject owner, GameObject target)
        {
            castTime = flyTime;
            enemy = owner.GetComponent<Enemy>();
            startPoint = owner.transform.position;
            endPoint = target.transform.position;

            PrepareEnemy();
            InstantiateThrowableObject();
            InstantiateVisualIndicator();
            ThrowObject();
        }

        private void InstantiateThrowableObject()
        {
            throwableObject = Instantiate(throwableObjectPrefab);
            throwableObject.transform.position = startPoint;
        }

        public override void BeginCooldown(GameObject owner, GameObject target)
        {
            Destroy(indicator);
            Destroy(throwableObject.gameObject);
            
            enemy.agentController.agent.isStopped = false;
            enemy.castingAbility = false;
        }

        private void InstantiateVisualIndicator()
        {
            indicator = Instantiate(indicatorPrefab);
            indicator.transform.position = endPoint;
            _fill = indicator.GetComponent<IndicatorFill>();
        }

        private void PrepareEnemy()
        {
            enemy.castingAbility = true;
            enemy.agentController.agent.isStopped = true;
            enemy.transform.DOLookAt(endPoint, .4f);
            enemy.animator.SetTrigger(AnimationParameters.Attack);
        }
        
        private async void ThrowObject()
        {
            throwableObject.GetComponent<Rigidbody>().velocity = CalculateVelocity();
            DOTween.To(() => _fill.fillProgress, x => _fill.fillProgress = x, 1f, flyTime);
            await UniTask.WaitForSeconds(flyTime);
            var pos = throwableObject.transform.position;
            pos.y = 1f;
            Instantiate(particle, pos, Quaternion.identity).Play();
        }

        private Vector3 CalculateVelocity()
        {
            Vector3 direction = endPoint - startPoint;
            Vector3 directionXZ = direction.normalized; 
            directionXZ.y = 0;
        
            //Vx = x / t
            float Vxz = direction.magnitude / flyTime;
        
            //Vy0 = y/t + 1/2 * g * t
            float Vy = direction.y / flyTime + 0.5f * Mathf.Abs(Physics.gravity.y) * flyTime;

            Vector3 result = directionXZ * Vxz;
            result.y = Vy;
            
            return result;
        }
    }
}
