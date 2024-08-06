using System;
using System.Collections.Generic;
using System.Globalization;
using Gameplay.Player;
using NaughtyAttributes;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.InventorySystem.UI
{
    public class InventoryManagerUI : MonoBehaviour
    {
        public RarityColorsFramesInfo rarityInfo;
        
        [Header("Slots")] [HorizontalLine(color: EColor.White, height: 1f)] [Space(10)]
        public int maxSlotCount;
        public GameObject inventorySlotPrefab;
        public GameObject inventoryEquipments;
        public GameObject inventoryOthers;

        [Header("Equipment Slots")] [HorizontalLine(color: EColor.White, height: 1f)] [Space(10)]
        [InfoBox("Place in order! 0=Head, 1=Chest, 2=Shoes, 3=Shoulder Pad, 4=Gloves, 5=Weapon")]
        public List<GameObject> equipments;
        
        [Header("Popup")] [HorizontalLine(color: EColor.White, height: 1f)] [Space(10)]
        public GameObject equipPopupPanel;
        public GameObject equipButton;
        public GameObject unequipButton;
        public Image icon;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        private InventorySlot _lastPopupSlot;
        private List<GameObject> _equipmentInventorySlots;
        private List<GameObject> _othersInventorySlots;

        [Header("Player Stats")] [HorizontalLine(color: EColor.White, height: 1f)] [Space(10)]
        public PlayerStats stats;
        public TextMeshProUGUI attack;
        public TextMeshProUGUI defence;
        public TextMeshProUGUI health;
        public TextMeshProUGUI missChance;
        public TextMeshProUGUI runSpeed;
        public TextMeshProUGUI walkSpeed;
        public TextMeshProUGUI attackCooldown;

        public static Action<InventorySlot, SlotType> OpenEquipPopup;
        
        private void Start()
        {
            Inventory.Instance.RefreshUI += OnRefreshUI;
            OpenEquipPopup += OnOpenEquipPopup;

            _equipmentInventorySlots = new List<GameObject>();
            _othersInventorySlots = new List<GameObject>();
            
            InstantiateSlots();
            OnRefreshUI();
        }

        private void InstantiateSlots()
        {
            for (int i = 0; i < maxSlotCount; i++)
            {
                _equipmentInventorySlots.Add(Instantiate(inventorySlotPrefab, inventoryEquipments.transform));
                _othersInventorySlots.Add(Instantiate(inventorySlotPrefab, inventoryOthers.transform));
            }
        }

        private void ClearInventoryUI()
        {
            for (int i = 0; i < maxSlotCount; i++)
            {
                _equipmentInventorySlots[i].SetActive(false);
                _othersInventorySlots[i].SetActive(false);
            }
        }

        private void OnRefreshUI()
        {
            ClearInventoryUI();

            for (int i = 0; i < Inventory.Instance.inventorySlots.Count; i++)
            {
                var slot = Inventory.Instance.inventorySlots[i];
                if (slot.item.itemType == ItemType.Others)
                {
                    _othersInventorySlots[i].GetComponent<InventorySlotUI>().Init(slot, rarityInfo);
                    _othersInventorySlots[i].SetActive(true);
                }
                else
                {
                    _equipmentInventorySlots[i].GetComponent<InventorySlotUI>().Init(slot, rarityInfo);
                    _equipmentInventorySlots[i].SetActive(true);
                }
            }

            for (int i = 0; i < Inventory.Instance.equipmentSlots.Count; i++)
            {
                var slot = Inventory.Instance.equipmentSlots[i];
                if (slot == null) { continue; }
                equipments[i].GetComponent<InventorySlotUI>().Init(slot, rarityInfo);
            }

            attack.text = stats.damage.Value.ToString();
            defence.text = stats.armor.Value.ToString();
            health.text = stats.maxHealth.Value.ToString();
            missChance.text = stats.missChance.Value.ToString(CultureInfo.CurrentCulture);
            runSpeed.text = stats.runningSpeed.Value.ToString(CultureInfo.CurrentCulture);
            walkSpeed.text = stats.walkingSpeed.Value.ToString(CultureInfo.CurrentCulture);
            attackCooldown.text = stats.attackCooldown.Value.ToString(CultureInfo.CurrentCulture);
        }

        private void OnOpenEquipPopup(InventorySlot slot, SlotType slotType)
        {
            _lastPopupSlot = slot;
            icon.sprite = slot.item.icon;
            titleText.text = slot.item.title;
            titleText.color = rarityInfo.rarityColors[(int)slot.item.itemRarity];
            descriptionText.text = slot.item.description;
            equipPopupPanel.SetActive(true);
            if (slotType == SlotType.InventorySlot)
            {
                equipButton.SetActive(true);
                unequipButton.SetActive(false);
            }
            else
            {
                equipButton.SetActive(false);
                unequipButton.SetActive(true);
            }
        }

        public void OnPopupCloseButtonPressed()
        {
            equipPopupPanel.SetActive(false);
            _lastPopupSlot = null;
        }

        public void OnEquipButtonPressed()
        {
            Inventory.Instance.Equip(_lastPopupSlot);
            equipPopupPanel.SetActive(false);
        }
        
        public void OnUnequipButtonPressed()
        {
            Inventory.Instance.RemoveFromEquipment(_lastPopupSlot);
            equipPopupPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            Inventory.Instance.RefreshUI -= OnRefreshUI;
            OpenEquipPopup -= OnOpenEquipPopup;
        }
    }
}
