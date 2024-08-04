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
        public GameObject targetIndicator;

        [HideInInspector] public bool canSee;
        [HideInInspector] public GameObject targetObject;

        private void Start()
        {
            FOVTask();
        }

        private void Update()
        {
            if (targetObject == null) { return; }

            var pos = targetObject.transform.position;
            pos.y = 0.05f;
            targetIndicator.transform.position = pos;
        }

        private async void FOVTask()
        {
            while (true)
            {
                var isCanceled = await UniTask.WaitForSeconds(
                    duration: 0.2f,
                    cancellationToken: this.GetCancellationTokenOnDestroy())
                    .SuppressCancellationThrow();
                if (isCanceled) { return; }
                
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
                            //targetIndicator.transform.SetParent(targetObject.transform, false);
                            targetIndicator.SetActive(true);
                            
                            return;
                        }
                    }
                    else
                    {
                        targetObject = null;
                        targetIndicator.SetActive(false);
                        canSee = false;
                    }
                }
            }
            else if (canSee)
            {
                targetObject = null;
                targetIndicator.SetActive(false);
                canSee = false;
            }
        }
    }
}
