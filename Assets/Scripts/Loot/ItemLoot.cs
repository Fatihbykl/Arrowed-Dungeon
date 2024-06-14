using InventorySystem;
using UnityEngine;

namespace Loot
{
    public class ItemLoot : LootDrop
    {
        public Item item;
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player"))
            {
                Inventory.Instance.AddItem(item, 1);
                Destroy(gameObject);
            }
        }
    }
}
