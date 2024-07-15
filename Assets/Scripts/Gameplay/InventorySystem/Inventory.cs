using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.AbilitySystem;
using UnityEngine;

namespace Gameplay.InventorySystem
{
    public class Inventory : MonoBehaviour
    {
        public List<InventorySlot> inventorySlots;
        // 0=Head, 1=Chest, 2=Shoes, 3=ShoulderPad, 4=Gloves, 5=Weapon
        public List<InventorySlot> equipmentSlots;
        public InventorySlot[] defaultSlots;
        public AbilityBase[] skills;
        public static Inventory Instance { get; private set; }
        public Item[] testItems;

        public event Action RefreshUI;
        public event Action EquipmentChanged;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Found more than one Inventory in the scene.");
                Destroy(this);
                return;
            }
            Instance = this;

            equipmentSlots = new List<InventorySlot>
            {
                defaultSlots[0], defaultSlots[1], defaultSlots[2], defaultSlots[3], defaultSlots[4], defaultSlots[5]
            };

            foreach (var testItem in testItems)
            {
                AddItem(testItem, 2);
            }
        }

        public void AddItem(Item item, int count)
        {
            var slot = GetInventorySlot(item);
            if (slot == null)
            {
                var newSlot = new InventorySlot(item.GetCopy(), count);
                inventorySlots.Add(newSlot);
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
            if (slot.itemCount <= 0)
            {
                slot.item.Destroy();
                inventorySlots.Remove(slot);
            }
            
            RefreshUI?.Invoke();
        }

        public void Equip(InventorySlot slot)
        {
            if (slot.item.itemType == ItemType.Others) { return; }

            var index = (int)slot.item.itemType;
            var equipSlot = equipmentSlots.ElementAtOrDefault(index);
            if (equipSlot.item.id != "dummy")
            {
                AddItem(equipSlot.item, 1);
            }
            equipmentSlots[index] = slot;
            RemoveItem(slot.item, 1);

            EquipmentChanged?.Invoke();
            RefreshUI?.Invoke();
        }

        public void RemoveFromEquipment(InventorySlot slot)
        {
            var index = (int)slot.item.itemType;
            equipmentSlots[index] = defaultSlots[index];
            AddItem(slot.item, 1);

            EquipmentChanged?.Invoke();
            RefreshUI?.Invoke();
        }

        public InventorySlot GetInventorySlot(Item item)
        {
            return inventorySlots.FirstOrDefault(slot => slot.item.id == item.id);
        }

        public bool HasId(string id)
        {
            return inventorySlots.Any(slot => slot.item.id == id);
        }
    }
}
