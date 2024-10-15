using Cinemachine;
using UnityEngine;

namespace Gameplay.Player
{
    public class CameraAngleChanger : MonoBehaviour
    {
        public CinemachineVirtualCamera cam;
        public float cameraRotationX;
        public float cameraBodyZ;
        public int triggerLayer;
        
        
        private bool _isAngleChanged;
        private int _activeColliderCount;
        private CinemachineTransposer _transposer;
        private Transform _cameraTransform;

        private void Start()
        {
            _transposer = cam.GetCinemachineComponent<CinemachineTransposer>();
            _cameraTransform = Camera.main.transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != triggerLayer) { return; }
            
            _activeColliderCount++;

            if (!_isAngleChanged && IsCameraViewBlocked())
            {
                cam.Priority = 5;
                _isAngleChanged = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != triggerLayer) { return; }
            
            _activeColliderCount--;

            if (_activeColliderCount == 0 || !IsCameraViewBlocked())
            {
                cam.Priority = 10;
                _isAngleChanged = false;
            }
        }
        
        private bool IsCameraViewBlocked()
        {
            var direction = transform.position - cam.transform.position;
            var distance = Vector3.Distance(transform.position, cam.transform.position);

            return Physics.Raycast(cam.transform.position, direction, out _, distance, 1 << triggerLayer);
        }
    }
}
