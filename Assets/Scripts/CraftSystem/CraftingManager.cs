using System;
using System.Collections.Generic;
using InventorySystem;
using InventorySystem.UI;
using UnityEngine;

namespace CraftSystem
{
    public class CraftingManager : MonoBehaviour
    {
        public CraftingRecipe[] recipes;
        public GameObject content;
        public GameObject recipeSlotPrefab;

        private List<GameObject> _slots;

        private void Start()
        {
            _slots = new List<GameObject>();
            
            InstantiateSlots();
        }

        private void InstantiateSlots()
        {
            for (int i = 0; i < recipes.Length; i++)
            {
                var slotUI = Instantiate(recipeSlotPrefab, content.transform);
                var recipe = recipes[i].GetCopy();

                slotUI.GetComponent<CraftingSlotUI>().recipe = recipe;
                slotUI.GetComponent<CraftingSlotUI>().Init();
                
                _slots.Add(slotUI);
            }
        }
    }
}
