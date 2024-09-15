using System;

namespace Gameplay.InventorySystem
{
    [Serializable]
    public class InventorySlot
    {
        [Newtonsoft.Json.JsonIgnore]
        public Item item;
        public int itemCount;

        public string ItemName => item.name;

        public InventorySlot(Item item, int itemCount)
        {
            this.item = item;
            this.itemCount = itemCount;
        }
        
        public void IncreaseCount(int count)
        {
            itemCount += count;
        }
        
        public void DecreaseCount(int count)
        {
            itemCount -= count;
        }
    }
}
