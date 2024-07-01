using UnityEngine;

namespace Gameplay.Managers
{
    public class ExperienceManager : MonoBehaviour
    {
        public static ExperienceManager Instance { get; private set; }
        
        [SerializeField] private AnimationCurve experienceCurve;

        public int _currentLevel, _totalExperience, _previousLevelExperience, _nextLevelExperience;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Found more than one Inventory in the scene.");
                Destroy(this);
                return;
            }
            Instance = this;

            UpdateVariables();
        }

        public void AddExperience(int amount)
        {
            _totalExperience += amount;
            CheckForLevelUp();
        }
        
        void Update() 
        {
            if(Input.GetMouseButtonDown(0))
            {
                AddExperience(5);
            }
        }

        void CheckForLevelUp()
        {
            if(_totalExperience >= _nextLevelExperience)
            {
                _currentLevel++;
                UpdateVariables();

                // Start level up sequence... Possibly vfx?
            }
        }

        void UpdateVariables()
        {
            _previousLevelExperience = (int)experienceCurve.Evaluate(_currentLevel);
            _nextLevelExperience = (int)experienceCurve.Evaluate(_currentLevel + 1);
        }
    }
}
