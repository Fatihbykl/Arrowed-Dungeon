using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay
{
    public class FieldOfView : MonoBehaviour
    {
        [Range(0, 360)] public float radius;
        public float angle;
        public LayerMask targetMask;
        public LayerMask obstructionMask;

        [HideInInspector] public bool canSee;
        [HideInInspector] public GameObject targetObject;

        private void Start()
        {
            FOVTask();
        }

        private async void FOVTask()
        {
            while (true)
            {
                await UniTask.WaitForSeconds(0.2f);
                FieldOfViewCheck();
            }
        }

        private void FieldOfViewCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

            if (rangeChecks.Length > 0)
            {
                var targets = rangeChecks.OrderBy(n => (n.transform.position - transform.position).sqrMagnitude)
                    .ToList();
                bool isContainEnemy = targets.Any(t => t.gameObject.layer == LayerMask.NameToLayer("Enemy"));
                
                foreach (var target in targets)
                {
                    if (isContainEnemy && target.gameObject.layer == LayerMask.NameToLayer("Breakable"))
                    {
                        continue;
                    }
                    Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

                    if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
                        if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                        {
                            canSee = true;
                            targetObject = target.gameObject;
                            return;
                        }
                    }
                    else
                    {
                        targetObject = null;
                        canSee = false;
                    }
                }
            }
            else if (canSee)
            {
                targetObject = null;
                canSee = false;
            }
        }
    }
}
