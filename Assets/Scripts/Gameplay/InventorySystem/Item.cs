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
        Common,
        Rare,
        Epic,
        Legendary,
        Mythic
    }
    
    [CreateAssetMenu(menuName = "Custom/Player Items/Item")]
    public class Item : ScriptableObject
    {
        public Sprite icon;
        public string id;
        public string title;
        [TextArea]
        public string description;
        public ItemType itemType;
        public ItemRarity itemRarity;
        public int clothIndex;
        public StatModifier[] modifiers;
        
        [InfoBox("Prefab for instantiating in game.")]
        public GameObject prefab;

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
