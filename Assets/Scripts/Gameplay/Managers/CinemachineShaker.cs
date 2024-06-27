using System;
using Cinemachine;
using UnityEngine;

namespace Gameplay.Managers
{
    public class CinemachineShaker : MonoBehaviour
    {
        public static CinemachineShaker Instance { get; private set; }

        private CinemachineVirtualCamera _cinemachineVirtualCamera;
        private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;
        private float _duration;
        private float _intensity;
        private float _shakeTimer;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Found more than one CinemachineShaker in the scene.");
                Destroy(this);
                return;
            }
            Instance = this;
            
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
            _multiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void ShakeCamera(float intensity, float time)
        {
            _duration = time;
            _intensity = intensity;
            _shakeTimer = time;
            
            _multiChannelPerlin.m_AmplitudeGain = intensity;
        }

        private void Update()
        {
            if (_shakeTimer > 0)
            {
                _shakeTimer -= Time.deltaTime;
                _multiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(_intensity, 0f, 1 - (_shakeTimer / _duration));
            }
        }
    }
}
