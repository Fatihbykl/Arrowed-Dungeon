using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public enum SlotType
    {
        EquipmentSlot,
        InventorySlot
    }
    public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
    {
        public InventorySlot slot;
        public SlotType slotType;
        public Image frameBackground;
        public Image frameEdge;
        public Image icon;
        public GameObject countObject;

        public void Init(InventorySlot inventorySlot)
        {
            slot = inventorySlot;
            icon.sprite = slot.item.icon;
            countObject.GetComponentInChildren<TextMeshProUGUI>().text = slot.itemCount.ToString();

            if (slotType == SlotType.EquipmentSlot) { countObject.SetActive(false); }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (slot.item.id == "dummy") { return; }
            InventoryManagerUI.OpenEquipPopup?.Invoke(slot, slotType);
        }
    }
}