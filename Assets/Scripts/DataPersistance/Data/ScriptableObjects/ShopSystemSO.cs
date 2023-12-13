using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShopSystem
{
    [CreateAssetMenu(fileName = "New Shop Item", menuName = "Shop Item")]
    [System.Serializable]
    public class ShopItemSO : ScriptableObject
    {
        public ItemData[] itemData;
    }

    [System.Serializable]
    public class ItemData
    {
        public enum ShopItemCategory { Upgrades, Skills }
        public enum ItemTitle { Health, Shield, Speed, Freeze, Immortal, Destroyer }

        public ItemTitle title;
        public string description;
        public Sprite icon;
        public ShopItemCategory category;
        public int currentLevel;
        public ItemLevelData[] levelData;

        public ItemLevelData getCurrentUpgradeData()
        {
            if (currentLevel == levelData.Length)
            {
                return null;
            }
            return levelData[currentLevel];
        }

        public void BuyItem()
        {
            if (category == ShopItemCategory.Skills)
            {
                ShopEvents.SkillBought.Invoke(title);
            }
            else
            {
                ShopEvents.ItemUpgraded.Invoke(title, levelData[currentLevel].value);
                if (currentLevel < levelData.Length) { currentLevel++; }
            }
        }
    }

    [System.Serializable]
    public class ItemLevelData
    {
        public int cost;
        public float value;
    }
}
