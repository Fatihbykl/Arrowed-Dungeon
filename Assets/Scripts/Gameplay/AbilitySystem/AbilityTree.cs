using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DataPersistance;
using DataPersistance.Data;
using Events;
using Gameplay.InventorySystem;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.AbilitySystem
{
    public class AbilityTree : MonoBehaviour, IDataPersistence
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
        private List<AbilityBase> _equippedAbilities = new(3) { null, null, null };

        private void Start()
        {
            EventManager.StartListening(EventStrings.AbilityNodeClicked, OnAbilityNodeClicked);

            //_equippedAbilities = new List<AbilityBase>(3) { null, null, null };
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

        private void UpdateNodeUI()
        {
            foreach (var node in nodes)
            {
                node.UnlockIfRequirementsMet();
            }
        }

        private void UpdateEquippedSlotsUI()
        {
            for (int i = 0; i < _equippedAbilities.Count; i++)
            {
                if (_equippedAbilities[i] != null)
                {
                    equipSlots[i].sprite = _equippedAbilities[i].icon;
                    equipSlots[i].color = Color.white;
                }
                else
                {
                    equipSlots[i].sprite = null;
                    equipSlots[i].color = Color.clear;
                }
            }
        }
        
        public void EquipAbility()
        {
            popupPanel.SetActive(false);
            
            for (int i = 0; i < _equippedAbilities.Count; i++)
            {
                if (_equippedAbilities[i] == null)
                {
                    _equippedAbilities[i] = _lastSender.ability;
                    _lastSender.EquipAbility();
                    
                    UpdateInventory();
                    UpdateEquippedSlotsUI();
                    
                    return;
                }
            }
        }
        
        public void UnEquipAbility()
        {
            popupPanel.SetActive(false);
            
            for (int i = 0; i < _equippedAbilities.Count; i++)
            {
                if (_equippedAbilities[i] == _lastSender.ability)
                {
                    _equippedAbilities[i] = null;
                    _equippedAbilities = _equippedAbilities.OrderByDescending(q=>q).ToList();
                    _lastSender.UnEquipAbility();
                    
                    UpdateInventory();
                    UpdateEquippedSlotsUI();
                    
                    return;
                }
            }
        }

        private void UpdateInventory()
        {
            Inventory.Instance.skills = _equippedAbilities;
        }

        public void BuyAbility()
        {
            _lastSender.BuyAbility();
            UpdateNodeUI();
            ClosePopup();
        }

        public void ClosePopup()
        {
            popupPanel.SetActive(false);
        }

        public bool IsLoaded { get; set; }

        public void LoadData(GameData data)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                var nodeData = data.abilityData.abilityTreeInfo[i];
                nodes[i].IsBought = nodeData.isBought;
                nodes[i].IsUnlocked = nodeData.isUnlocked;
                nodes[i].IsEquipped = nodeData.isEquipped;
            }
            
            for (var i = 0; i < data.abilityData.equippedSkills.Count; i++)
            {
                var skillName = data.abilityData.equippedSkills[i];
                var path = $"Abilities/Player/{skillName}";
                var skill = Resources.Load<AbilityBase>(path);

                _equippedAbilities[i] = skill;
            }
            
            UpdateNodeUI();
            UpdateEquippedSlotsUI();
        }

        public void SaveData(GameData data)
        {
            var equippedSkills = _equippedAbilities
                .Where(ability => ability != null)
                .Select(ability => ability.name).ToList();

            var abilityNodes = nodes.Select(
                node => new AbilityTreeNodeInfo(node.IsUnlocked, node.IsBought, node.IsEquipped)
            ).ToList();
            
            data.abilityData.equippedSkills = equippedSkills;
            data.abilityData.abilityTreeInfo = abilityNodes;
        }
    }
}
