using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class InventoryManagerUI : MonoBehaviour
    {
        public int maxSlotCount;
        public GameObject inventorySlotPrefab;
        public GameObject inventoryEquipments;
        public GameObject inventoryOthers;

        [FormerlySerializedAs("popupPanel")] [Header("Popup")] 
        public GameObject equipPopupPanel;
        public GameObject equipButton;
        public GameObject unequipButton;
        public Image icon;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        private InventorySlot _lastPopupSlot;
        private List<GameObject> _equipmentInventorySlots;
        private List<GameObject> _othersInventorySlots;

        public static Action<InventorySlot, SlotType> OpenEquipPopup;

        [InfoBox("Place in order! 0=Head, 1=Chest, 2=Shoes, 3=Shoulder Pad, 4=Gloves, 5=Weapon")]
        public List<GameObject> equipments;
        public InventorySlot[] emptyEquipmentSlots;

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

            for (int i = 0; i < equipments.Count; i++)
            {
                equipments[i].GetComponent<InventorySlotUI>().Init(emptyEquipmentSlots[i]);
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
                    _othersInventorySlots[i].GetComponent<InventorySlotUI>().Init(slot);
                    _othersInventorySlots[i].SetActive(true);
                }
                else
                {
                    _equipmentInventorySlots[i].GetComponent<InventorySlotUI>().Init(slot);
                    _equipmentInventorySlots[i].SetActive(true);
                }
                
            }

            for (int i = 0; i < Inventory.Instance.equipmentSlots.Count; i++)
            {
                var slot = Inventory.Instance.equipmentSlots[i];
                if (slot == null) { continue; }
                equipments[i].GetComponent<InventorySlotUI>().Init(slot);
            }
        }

        private void OnOpenEquipPopup(InventorySlot slot, SlotType slotType)
        {
            _lastPopupSlot = slot;
            icon.sprite = slot.item.icon;
            titleText.text = slot.item.title;
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
    }
}
