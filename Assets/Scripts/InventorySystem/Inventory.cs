using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public List<InventorySlot> inventorySlots;
        // 0=Head, 1=Chest, 2=Shoes, 3=ShoulderPad, 4=Gloves, 5=Weapon
        public List<InventorySlot> equipmentSlots;
        public static Inventory Instance { get; private set; }
        public Item[] testItems;

        public event Action RefreshUI;
        public event Action StatsChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Found more than one Inventory in the scene.");
                Destroy(this);
                return;
            }
            Instance = this;

            equipmentSlots = new List<InventorySlot> { null, null, null, null, null, null };

            foreach (var testItem in testItems)
            {
                AddItem(testItem, 3);
                //Equip(inventorySlots.GetValueOrDefault(testItem.id));
            }
        }

        public void AddItem(Item item, int count)
        {
            var slot = GetInventorySlot(item);
            if (slot == null)
            {
                var newSlot = new InventorySlot(item, count);
                inventorySlots.Add(newSlot);
                // added new slot
            }
            else
            {
                slot.IncreaseCount(count);
            }
            RefreshUI?.Invoke();
        }

        public void RemoveItem(Item item, int count)
        {
            var slot = GetInventorySlot(item);
            if (slot == null) { return; }
            
            slot.DecreaseCount(count);
            if (slot.itemCount <= 0) { inventorySlots.Remove(slot); }
            
            RefreshUI?.Invoke();
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
            equipmentSlots[index] = slot;
            RemoveItem(slot.item, 1);
            
            RefreshUI?.Invoke();
            StatsChanged?.Invoke();
        }

        public void RemoveFromEquipment(InventorySlot slot)
        {
            equipmentSlots[(int)slot.item.itemType] = null;
            AddItem(slot.item, 1);
            
            RefreshUI?.Invoke();
            StatsChanged?.Invoke();
        }

        private InventorySlot GetInventorySlot(Item item)
        {
            return inventorySlots.FirstOrDefault(slot => slot.item.id == item.id);
        }
    }
}
