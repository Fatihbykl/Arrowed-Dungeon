using System;
using Events;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.AbilitySystem
{
    public class AbilityTree : MonoBehaviour
    {
        public AbilityTreeNode[] nodes;
        public Image[] equipSlots;

        [Header("Popup")] [HorizontalLine(color: EColor.White, height: 1f)] [Space(10)]
        public GameObject popupPanel;
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;
        public Image icon;
        public GameObject equipButton;
        public GameObject unequipButton;
        public GameObject lockedText;
        public GameObject buyButton;

        private AbilityTreeNode _lastSender;
        private AbilityBase[] _equippedAbilities = new AbilityBase[3];

        private void Start()
        {
            EventManager.StartListening(EventStrings.AbilityNodeClicked, OnAbilityNodeClicked);
        }

        private void OnDestroy()
        {
            EventManager.StopListening(EventStrings.AbilityNodeClicked, OnAbilityNodeClicked);
        }

        public void OnAbilityNodeClicked()
        {
            var sender = (AbilityTreeNode)EventManager.GetSender(EventStrings.AbilityNodeClicked);
            
            equipButton.SetActive(false);
            unequipButton.SetActive(false);
            lockedText.SetActive(false);
            buyButton.SetActive(false);

            if (sender.IsBought)
            {
                if (sender.IsEquipped) { unequipButton.SetActive(true); }
                else { equipButton.SetActive(true); }
            }
            else if (sender.IsUnlocked) { buyButton.SetActive(true); }
            else { lockedText.SetActive(true); }

            title.text = sender.ability.title;
            description.text = sender.ability.description;
            icon.sprite = sender.ability.icon;

            popupPanel.SetActive(true);
            _lastSender = sender;
        }

        private void UpdateUI()
        {
            foreach (var node in nodes)
            {
                node.UnlockIfRequirementsMet();
            }
        }
        
        public void EquipAbility()
        {
            popupPanel.SetActive(false);
            
            for (int i = 0; i < equipSlots.Length; i++)
            {
                if (equipSlots[i].sprite == null)
                {
                    equipSlots[i].sprite = _lastSender.ability.icon;
                    equipSlots[i].color = Color.white;
                    _equippedAbilities[i] = _lastSender.ability;
                    _lastSender.EquipAbility();
                    return;
                }
            }
        }
        
        public void UnEquipAbility()
        {
            popupPanel.SetActive(false);
            
            for (int i = 0; i < equipSlots.Length; i++)
            {
                if (_equippedAbilities[i] == _lastSender.ability)
                {
                    equipSlots[i].sprite = null;
                    equipSlots[i].color = Color.clear;
                    _equippedAbilities[i] = null;
                    _lastSender.UnEquipAbility();
                    return;
                }
            }
        }

        public void BuyAbility()
        {
            _lastSender.BuyAbility();
            UpdateUI();
            ClosePopup();
        }

        public void ClosePopup()
        {
            popupPanel.SetActive(false);
        }
    }
}
