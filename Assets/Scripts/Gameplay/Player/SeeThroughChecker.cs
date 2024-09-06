using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Player
{
    public class SeeThroughChecker : MonoBehaviour
    {
        public GameObject stencilMask;

        private void Start()
        {
            CheckIfNeedMask();
        }

        private async void CheckIfNeedMask()
        {
            while (true)
            {
                var isCanceled = await UniTask.WaitForSeconds(
                        duration: 0.2f,
                        cancellationToken: this.GetCancellationTokenOnDestroy())
                    .SuppressCancellationThrow();
                if (isCanceled) { return; }
                
                RayCastToPlayer();
            }
        }

        private void RayCastToPlayer()
        {
            const int layerMask = 1 << 15;
            
            var cameraTransform = Camera.main.transform;
            var direction = transform.position - cameraTransform.position;
            var distance = Vector3.Distance(transform.position, cameraTransform.position);

            if (Physics.Raycast(cameraTransform.position, direction, out _, distance, layerMask))
            {
                stencilMask.SetActive(true);
            }
            else
            {
                stencilMask.SetActive(false);
            }
        }
    }
}
