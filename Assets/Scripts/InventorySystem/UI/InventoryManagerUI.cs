using System;
using UnityEngine;

namespace InventorySystem.UI
{
    public class InventoryManagerUI : MonoBehaviour
    {
        public GameObject inventorySlotPrefab;
        public GameObject inventoryEquipments;
        public GameObject inventoryOthers;

        private void Start()
        {
            foreach (var (id, slot) in Inventory.Instance.inventorySlots)
            {
                var prefab = Instantiate(inventorySlotPrefab);
                
                if (slot.item.itemType == ItemType.Others) { prefab.transform.SetParent(inventoryOthers.transform, false); }
                else { prefab.transform.SetParent(inventoryEquipments.transform, false); }
                
                var slotUI = prefab.GetComponent<InventorySlotUI>();
                slotUI.slot = slot;
                slotUI.UpdateUI();
            }
        }
    }
}
