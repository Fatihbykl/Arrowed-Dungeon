using Gameplay.Player;
using Gameplay.StatSystem;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.InventorySystem
{
    public enum ItemType
    {
        Head = 0,
        Chest = 1,
        Shoes = 2,
        ShoulderPad = 3,
        Gloves = 4,
        Weapon = 5,
        Others = 6
    }

    public enum ItemRarity
    {
        Common = 0,
        Rare = 1,
        Epic = 2,
        Legendary = 3,
        Mythic = 4,
        None = 5
    }
    
    [CreateAssetMenu(menuName = "Custom/Player Items/Item")]
    public class Item : ScriptableObject
    {
        [ShowAssetPreview]
        public Sprite icon;
        public string id;
        public string title;
        [TextArea]
        public string description;
        public Cost cost;
        public ItemType itemType;
        public ItemRarity itemRarity;
        public int clothIndex;
        public StatModifier[] modifiers;

        public Item GetCopy()
        {
            return Instantiate(this);
        }

        public void Destroy()
        {
            Destroy(this);
        }
    }
}
