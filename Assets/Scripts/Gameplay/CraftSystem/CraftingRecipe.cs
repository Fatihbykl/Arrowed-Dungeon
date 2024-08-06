using System.Collections.Generic;
using Gameplay.InventorySystem;
using Gameplay.Player;
using UnityEngine;

namespace Gameplay.CraftSystem
{
    public enum CraftStates
    {
        Success,
        NotEnoughMaterial,
        Failure
    }
    
    [CreateAssetMenu(menuName = "Custom/Crafting/Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        public List<InventorySlot> materials;
        public InventorySlot result;
        public Cost craftCost;
        [Range(0, 100)]
        public int craftChance;

        public CraftStates Craft(int extraLuck)
        {
            var state = CanCraft(extraLuck);
            if (state != CraftStates.Success) return state;
            
            RemoveMaterials();
            AddResults();
            
            return CraftStates.Success;
        }

        private CraftStates CanCraft(int extraLuck)
        {
            if (HasMaterials())
            {
                return IsCraftSuccessful(extraLuck) ? CraftStates.Success : CraftStates.Failure;
            }

            return CraftStates.NotEnoughMaterial;
        }

        private bool IsCraftSuccessful(int extraLuck)
        {
            var totalChance = craftChance + extraLuck;
            if (totalChance >= 100) { return true; }

            return Random.Range(0, 100) <= totalChance;
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
