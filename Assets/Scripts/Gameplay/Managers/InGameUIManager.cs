using System;
using System.Linq;
using Events;
using Gameplay.AbilitySystem;
using UI.Joystick.Joysticks;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Managers
{
    public class InGameUIManager : MonoBehaviour
    {
        public GameObject pauseScreen;
        public GameObject levelPassedScreen;
        public GameObject levelLostScreen;
        public GameObject[] skillButtons;

        private EventsGroup _eventsGroup = new EventsGroup();
        private void Awake()
        {
            _eventsGroup.Add(EventStrings.GamePaused, OnGamePaused);
            _eventsGroup.Add(EventStrings.GameContinued, OnGameContinued);
            _eventsGroup.Add(EventStrings.LevelPassed, OnLevelPassed);
            _eventsGroup.Add(EventStrings.LevelLost, OnLevelLost);
            _eventsGroup.StartListening();
            
            PrepareSkills();
        }

        private void OnLevelLost()
        {
            levelLostScreen.SetActive(true);
        }

        private void OnLevelPassed()
        {
            levelPassedScreen.SetActive(true);
        }

        private void OnGameContinued()
        {
            pauseScreen.SetActive(false);
        }

        public void OnGamePaused()
        {
            pauseScreen.SetActive(true);
        }

        private void PrepareSkills()
        {
            var abilityHolders = GameManager.Instance.playerObject.GetComponents<AbilityHolder>().ToList();
            for (var i = 0; i < abilityHolders.Count; i++)
            {
                var abilityHolder = abilityHolders[i];
                var joystick = skillButtons[i].GetComponent<SkillFixedJoystick>();
                var icon = skillButtons[i].GetComponent<Image>();

                joystick.skillType = abilityHolder.ability.skillType;
                joystick.abilityHolder = abilityHolder;
                icon.sprite = abilityHolder.ability.icon;
                
                skillButtons[i].SetActive(true);
            }
        }
        
        private void OnDestroy()
        {
            _eventsGroup.StopListening();
        }
    }
}
