using Animations;
using DataPersistance;
using DataPersistance.Data;
using DataPersistance.Data.ScriptableObjects;
using Events;
using UnityEngine;

namespace Gameplay.Managers
{
    public class GameManager : MonoBehaviour
    {
        public GameObject playerObject;
        
        public GameObject gate;
        public SoundClip gateSound;

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 300;
            
            if (Instance != null)
            {
                Debug.LogError("Found more than one Game Manager in the scene.");
            }
            Instance = this;
        }

        public void PuzzleCompleted()
        {
            gate.GetComponent<Animator>().SetTrigger(AnimationParameters.OpenGate);
            //AudioManager.Instance.PlaySoundFXClip(gateSound, gate.transform);
        }
    }
}
