using System;
using Cinemachine;
using Gameplay.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.TangramGame
{
    public class TangramActivator : MonoBehaviour
    {
        public CinemachineVirtualCamera mainCam;
        public CinemachineVirtualCamera tangramCam;
        public GameObject mainUI;

        private bool _isOpen;
        
        private void Start()
        {
            GameManager.instance.playerObject.GetComponent<PlayerInput>().actions["OpenTangram"].performed +=
                x => OnOpenTangram();
        }

        private void OnOpenTangram()
        {
            if (_isOpen)
            {
                tangramCam.Priority = 9;
                mainUI.SetActive(true);
                _isOpen = false;
            }
            else
            {
                tangramCam.Priority = 11;
                mainUI.SetActive(false);
                _isOpen = true;
            }
            
        }
    }
}
