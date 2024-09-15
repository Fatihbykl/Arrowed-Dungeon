using System;
using System.Collections.Generic;
using System.Linq;
using DataPersistance;
using DataPersistance.Data;
using Gameplay.AbilitySystem;
using UnityEngine;

namespace Gameplay.InventorySystem
{
    public class Inventory : MonoBehaviour, IDataPersistence
    {
        public List<InventorySlot> inventorySlots;
        // 0=Head, 1=Chest, 2=Shoes, 3=ShoulderPad, 4=Gloves, 5=Weapon
        public List<InventorySlot> equipmentSlots;
        public InventorySlot[] defaultSlots;
        public List<AbilityBase> skills;
        public static Inventory Instance { get; private set; }

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
            DontDestroyOnLoad(gameObject);

            equipmentSlots = new List<InventorySlot>
            {
                defaultSlots[0], defaultSlots[1], defaultSlots[2], defaultSlots[3], defaultSlots[4], defaultSlots[5]
            };
        }

        public void AddItem(Item item, int count)
        {
            var slot = GetInventorySlot(item);
            if (slot == null)
            {
                var newSlot = new InventorySlot(item, count);
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
            if (slot == null || slot.itemCount == -1) { return; }
            
            slot.DecreaseCount(count);
            if (slot.itemCount <= 0)
            {
                //slot.item.Destroy();
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

        public bool IsLoaded { get; set; }

        public void LoadData(GameData data)
        {
            for (var i = 0; i < data.inventoryData.equipmentSlots.Count; i++)
            {
                var slot = data.inventoryData.equipmentSlots[i];
                var path = $"Items/{slot.itemName}";
                var item = Resources.Load<Item>(path);
                var inventorySlot = new InventorySlot(item, slot.itemCount);

                equipmentSlots[i] = inventorySlot;
            }

            if (data.inventoryData.inventorySlots == null) { return; }
            
            foreach (var slot in data.inventoryData.inventorySlots)
            {
                var path = $"Items/{slot.itemName}";
                var item = Resources.Load<Item>(path);
                var inventorySlot = new InventorySlot(item, slot.itemCount);
                
                inventorySlots.Add(inventorySlot);
            }
        }

        public void SaveData(GameData data)
        {
            var inventorySlotList = inventorySlots.Select(slot => new InventorySlotData(slot.ItemName, slot.itemCount)).ToList();
            var equipmentSlotList = equipmentSlots.Select(slot => new InventorySlotData(slot.ItemName, slot.itemCount)).ToList();
            
            data.inventoryData.inventorySlots = inventorySlotList;
            data.inventoryData.equipmentSlots = equipmentSlotList;
        }
    }
}
