using System;
using Events;
using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

namespace Gameplay.AbilitySystem
{
    public class AbilityTreeNode : MonoBehaviour, IPointerClickHandler
    {
        public AbilityBase ability;
        public AbilityTreeNode[] requirements;
        public int cost;
        public Image icon;
        public GameObject lockBg;
        public GameObject lockImage;

        public bool IsUnlocked { get; private set; }
        public bool IsBought { get; private set; }
        public bool IsEquipped { get; private set; }

        private void Start()
        {
            icon.sprite = ability.icon;
            UnlockIfRequirementsMet();
        }

        public void UnlockIfRequirementsMet()
        {
            if (requirements.Length == 0)
            {
                IsUnlocked = true;
                lockImage.SetActive(false);
                return;
            }
            
            foreach (var node in requirements)
            {
                if (!node.IsBought) { return; }
            }

            IsUnlocked = true;
            lockImage.SetActive(false);
        }

        public void EquipAbility()
        {
            IsEquipped = true;
        }

        public void UnEquipAbility()
        {
            IsEquipped = false;
        }

        public void BuyAbility()
        {
            lockBg.SetActive(false);
            IsBought = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventManager.EmitEvent(EventStrings.AbilityNodeClicked, this);
        }
    }
}
