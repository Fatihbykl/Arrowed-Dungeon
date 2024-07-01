using System.Collections.Generic;
using Gameplay.InventorySystem;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.CraftSystem
{
    [CreateAssetMenu(menuName = "Custom/Crafting/Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public List<InventorySlot> materials;
        public InventorySlot result;
        public Cost craftCost;

        public bool CanCraft()
        {
            return HasMaterials();
        }

        public void Craft()
        {
            if (CanCraft())
            {
                RemoveMaterials();
                AddResults();
            }
        }
        
        private bool HasMaterials()
        {
            foreach (InventorySlot slot in materials)
            {
                if (slot == null || Inventory.Instance.GetInventorySlot(slot.item).itemCount < slot.itemCount)
                {
                    Debug.LogWarning("You don't have the required items!");
                }
            }

            return true;
        }

        private void RemoveMaterials()
        {
            foreach (InventorySlot slot in materials)
            {
                Inventory.Instance.RemoveItem(slot.item, slot.itemCount);
            }
        }

        private void AddResults()
        {
            Inventory.Instance.AddItem(result.item, result.itemCount);
        }
        
        public CraftingRecipe GetCopy()
        {
            return Instantiate(this);
        }

        public void Destroy()
        {
            Destroy(this);
        }
    }
}
