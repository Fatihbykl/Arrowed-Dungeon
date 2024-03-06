using System;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        
        [Header("Gameplay Sounds")]
        [SerializeField] private SoundClip playerFootstep;
        [SerializeField] private SoundClip arrowWhoosh;
        [SerializeField] private SoundClip arrowImpact;
        [SerializeField] private SoundClip coinsDrop;
        
        public static AudioManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Found more than one Audio Manager in the scene.");
            }
            instance = this;
        }

        public void PlayFootstepSFX()
        {
            audioSource.PlayOneShot(playerFootstep.audioClip, playerFootstep.volume);
        }
        
        public void PlayArrowWooshSFX()
        {
            audioSource.PlayOneShot(arrowWhoosh.audioClip, arrowWhoosh.volume);
        }
        
        public void PlayArrowImpactSFX()
        {
            audioSource.PlayOneShot(arrowImpact.audioClip, arrowImpact.volume);
        }
        
        public void PlayArrowCoinsDropSFX()
        {
            audioSource.PlayOneShot(coinsDrop.audioClip, coinsDrop.volume);
        }
    }
    
    [Serializable]
    public class SoundClip
    {
        public AudioClip audioClip;
        [Range(0, 1)] public float volume;
    }
}
