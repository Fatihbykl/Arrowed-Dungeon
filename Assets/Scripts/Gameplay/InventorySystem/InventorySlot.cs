using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    [Serializable]
    public class InventorySlot
    {
        public Item item;
        public int itemCount;

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
