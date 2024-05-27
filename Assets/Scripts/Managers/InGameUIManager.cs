using System;
using System.Linq;
using AbilitySystem;
using InventorySystem;
using UI.Joystick.Joysticks;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class InGameUIManager : MonoBehaviour
    {
        public GameObject[] skillButtons;

        private void Start()
        {
            var abilityHolders = GameManager.instance.playerObject.GetComponents<AbilityHolder>().ToList();
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
    }
}
