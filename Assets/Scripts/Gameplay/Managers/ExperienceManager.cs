using System;
using Events;
using UnityEngine;

namespace Gameplay.Managers
{
    public class ExperienceManager : MonoBehaviour
    {
        public static ExperienceManager Instance { get; private set; }

        public int CurrentLevel { get; private set; }
        public int TotalExperience { get; private set; }
        public int NextLevelExperience { get; private set; }
        public int PreviousLevelExperience { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Found more than one Experience Manager in the scene.");
                Destroy(this);
                return;
            }
            Instance = this;

            CurrentLevel = 1;
            UpdateVariables();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                AddExperience(50);
            }
        }

        private int GetLevelExperience(int level)
        {
            // a_n = 25/3 (n^^3 + 6n^^2 + 5n)
            var exp = (25 / 3f) * (Mathf.Pow(level, 3) + 6 * Mathf.Pow(level, 2) + 5 * level);
            return Mathf.RoundToInt(exp);
        }

        public void AddExperience(int amount)
        {
            TotalExperience += amount;
            CheckForLevelUp();
        }

        void CheckForLevelUp()
        {
            if (TotalExperience < NextLevelExperience) return;
            
            CurrentLevel++;
            UpdateVariables();

            EventManager.EmitEvent(EventStrings.LevelUpgraded);
        }

        void UpdateVariables()
        {
            PreviousLevelExperience = GetLevelExperience(CurrentLevel - 1);
            NextLevelExperience = GetLevelExperience(CurrentLevel);
        }
    }
}
