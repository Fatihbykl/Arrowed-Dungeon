using Gameplay.InventorySystem;
using UnityEngine;

namespace Gameplay.Loot
{
    public class ItemLoot : LootDrop
    {
        public Item item;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Inventory.Instance.AddItem(item, 1);
                Destroy(gameObject);
            }
        }
    }
}
