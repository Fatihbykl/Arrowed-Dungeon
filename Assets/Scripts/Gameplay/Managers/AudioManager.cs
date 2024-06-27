using System;
using UnityEngine;

namespace Gameplay.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource audioObject;
        
        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Found more than one Audio Manager in the scene.");
            }
            Instance = this;
        }

        public void PlaySoundFXClip(SoundClip clip, Transform spawnTransform)
        {
            AudioSource audioSource = Instantiate(audioObject, spawnTransform.position, Quaternion.identity);

            audioSource.clip = clip.audioClip;
            audioSource.volume = clip.volume;
            audioSource.Play();

            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }
        
        public void PlayRandomSoundFXClip(SoundClip[] clips, Transform spawnTransform)
        {
            int rand = UnityEngine.Random.Range(0, clips.Length);
            var clip = clips[rand];
            
            AudioSource audioSource = Instantiate(audioObject, spawnTransform.position, Quaternion.identity);

            audioSource.clip = clip.audioClip;
            audioSource.volume = clip.volume;
            audioSource.Play();

            float clipLength = audioSource.clip.length;
            Destroy(audioSource.gameObject, clipLength);
        }
    }
    
    [Serializable]
    public class SoundClip
    {
        public AudioClip audioClip;
        [Range(0, 1)] public float volume;
    }
}
