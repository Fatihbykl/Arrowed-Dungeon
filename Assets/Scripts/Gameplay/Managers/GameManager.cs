using System;
using Animations;
using DataPersistance;
using DataPersistance.Data;
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
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            EventManager.StartListening(EventStrings.PuzzleCompleted, OnPuzzleCompleted);
            EventManager.StartListening(EventStrings.LevelLost, OnLevelLost);
        }

        private void OnDestroy()
        {
            EventManager.StopListening(EventStrings.PuzzleCompleted, OnPuzzleCompleted);
            EventManager.StopListening(EventStrings.LevelLost, OnLevelLost);

            Time.timeScale = 1f;
        }

        private void OnLevelLost()
        {
            Time.timeScale = 0;
        }

        public void PauseGame()
        {
            EventManager.EmitEvent(EventStrings.GamePaused);
            Time.timeScale = 0;
        }

        public void ContinueGame()
        {
            EventManager.EmitEvent(EventStrings.GameContinued);
            Time.timeScale = 1f;
        }

        public void OnPuzzleCompleted()
        {
            gate.GetComponent<Animator>().SetTrigger(AnimationParameters.OpenGate);
            //AudioManager.Instance.PlaySoundFXClip(gateSound, gate.transform);
        }
    }
}
