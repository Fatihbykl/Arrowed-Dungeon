using System.Collections.Generic;
using Gameplay.AbilitySystem;
using Gameplay.InventorySystem;
using JetBrains.Annotations;

namespace DataPersistance.Data
{
    public struct AbilityTreeNodeInfo
    {
        public bool isUnlocked;
        public bool isBought;
        public bool isEquipped;

        public AbilityTreeNodeInfo(bool unlocked, bool bought, bool equipped)
        {
            isUnlocked = unlocked;
            isBought = bought;
            isEquipped = equipped;
        }
    }

    public struct InventorySlotData
    {
        public string itemName;
        public int itemCount;

        public InventorySlotData(string name, int count)
        {
            itemName = name;
            itemCount = count;
        }
    }

    public struct InventoryData
    {
        public List<InventorySlotData> inventorySlots;
        public List<InventorySlotData> equipmentSlots;
    }

    public struct AbilityData
    {
        public List<string> equippedSkills;
        public List<AbilityTreeNodeInfo> abilityTreeInfo;
    }

    public struct CoinData
    {
        public int gold;
        public int gem;
    }
    
    [System.Serializable]
    public class GameData
    {
        public InventoryData inventoryData;
        public AbilityData abilityData;
        public CoinData coinData;
    }
}
