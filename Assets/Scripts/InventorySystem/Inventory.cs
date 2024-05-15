using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public Dictionary<string, InventorySlot> inventorySlots;
        // 0=Head, 1=Chest, 2=Shoes, 3=ShoulderPad, 4=Gloves, 5=Weapon
        public List<InventorySlot> equipmentSlots;
        public static Inventory Instance { get; private set; }
        public Item[] testItems;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Found more than one Inventory in the scene.");
                Destroy(this);
                return;
            }
            Instance = this;

            inventorySlots = new Dictionary<string, InventorySlot>();
            equipmentSlots = new List<InventorySlot>(6);

            foreach (var testItem in testItems)
            {
                AddItem(testItem, 3);
            }
        }

        public void AddItem(Item item, int count)
        {
            var slot = GetInventorySlot(item);
            if (slot == null)
            {
                var newSlot = new InventorySlot(item, count);
                inventorySlots.Add(item.id, newSlot);
                // added new slot
            }
            else
            {
                slot.IncreaseCount(count);
                // updated event?
            }
        }

        public void RemoveItem(Item item, int count)
        {
            var slot = GetInventorySlot(item);
            if (slot == null) { return; }
            
            slot.DecreaseCount(count);
            if (slot.itemCount <= 0) { inventorySlots.Remove(slot.item.id); }
        }

        public void Equip(InventorySlot slot)
        {
            if (slot.item.itemType == ItemType.Others) { return; }

            var index = (int)slot.item.itemType;
            var equipSlot = equipmentSlots.ElementAtOrDefault(index);
            if (equipSlot != null)
            {
                AddItem(equipSlot.item, 1);
            }
            equipmentSlots.Insert(index, slot);
            RemoveItem(slot.item, 1);
        }

        public void RemoveFromEquipment(InventorySlot slot)
        {
            equipmentSlots.RemoveAt((int)slot.item.itemType);
            AddItem(slot.item, 1);
        }

        private InventorySlot GetInventorySlot(Item item)
        {
            return inventorySlots.GetValueOrDefault(item.id);
        }
    }
}
