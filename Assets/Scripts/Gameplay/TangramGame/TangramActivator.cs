using System;
using Cinemachine;
using Gameplay.Managers;
using Gameplay.TangramGame.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Gameplay.TangramGame
{
    public class TangramActivator : MonoBehaviour
    {
        public CinemachineVirtualCamera mainCam;
        public CinemachineVirtualCamera tangramCam;
        public Camera realCamera;
        public LayerMask cullingMaskInTangram;
        public GameObject mainUI;
        public GameObject tangramUI;
        [FormerlySerializedAs("gameController")] public TangramGameController tangramGameController;

        private bool _isOpen;
        private LayerMask _defaultMask;

        private void Start()
        {
            _defaultMask = realCamera.cullingMask;
        }

        private void OnTriggerEnter(Collider other)
        {
            OpenTangram();
        }

        public void OpenTangram()
        {
            if (_isOpen) { return; }
            
            tangramCam.Priority = 11;
            mainUI.SetActive(false);
            tangramUI.SetActive(true);
            _isOpen = true;
            tangramGameController.ShowFoundPieces();
            realCamera.cullingMask = cullingMaskInTangram;
        }

        public void CloseTangram()
        {
            if (!_isOpen) { return; }
            
            tangramCam.Priority = 9;
            mainUI.SetActive(true);
            tangramUI.SetActive(false);
            _isOpen = false;
            realCamera.cullingMask = _defaultMask;
        }
    }
}
