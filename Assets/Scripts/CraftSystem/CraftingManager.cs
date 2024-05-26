using System;
using System.Collections.Generic;
using Gameplay.Player;
using InventorySystem;
using InventorySystem.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CraftSystem
{
    public class CraftingManager : MonoBehaviour
    {
        public CraftingRecipe[] recipes;
        public GameObject content;
        
        [Header("Recipe Details")]
        public GameObject recipeSlotPrefab;
        public GameObject recipeInfoParent;
        public GameObject recipeInfoPrefab;
        public int recipeInfoRowCount;
        public TextMeshProUGUI craftButtonCostText;

        [Header("After Craft Popup")] 
        public GameObject popup;
        public Image popupImage;
        public TextMeshProUGUI popupText;
        
        public static Action<CraftingSlotUI> RecipeClicked;
        
        private List<GameObject> _slots;
        private List<GameObject> _rows;
        private CraftingRecipe _lastClickedRecipe;

        private void Start()
        {
            RecipeClicked += OnRecipeClicked;
            
            _slots = new List<GameObject>();
            _rows = new List<GameObject>();
            
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

            for (int i = 0; i < recipeInfoRowCount; i++)
            {
                var row = Instantiate(recipeInfoPrefab, recipeInfoParent.transform);
                row.SetActive(false);

                _rows.Add(row);
            }
        }

        public void OnRecipeClicked(CraftingSlotUI slot)
        {
            _lastClickedRecipe = slot.recipe;
            ClearRecipeInfo();

            craftButtonCostText.text = slot.recipe.craftCost.amount.ToString();
            for (int i = 0; i < slot.recipe.materials.Count; i++){

                var countText = _rows[i].transform.Find("Count").GetComponent<TextMeshProUGUI>();
                _rows[i].transform.Find("Title").GetComponent<TextMeshProUGUI>().text = slot.recipe.materials[i].item.title;
                _rows[i].transform.Find("InventorySlot/Icon").GetComponent<Image>().sprite = slot.recipe.materials[i].item.icon;
                
                var material = slot.recipe.materials[i];
                var inventoryItem = Inventory.Instance.GetInventorySlot(material.item);
                
                countText.text = material.itemCount + "x";
                if (inventoryItem == null || inventoryItem.itemCount < material.itemCount)
                {
                    countText.color = Color.red;
                }
                else
                {
                    countText.color = Color.green;
                }
                
                _rows[i].SetActive(true);
            }
        }

        public void OnCraftButtonClicked()
        {
            if (_lastClickedRecipe == null) { return; }

            var bought = Coin.Instance.SpendCoin(_lastClickedRecipe.craftCost.amount,
                _lastClickedRecipe.craftCost.coinType);

            if (bought)
            {
                _lastClickedRecipe.Craft();
                OpenPopup();
            }
            else
            {
                Debug.LogWarning("Not enough coin!");
            }
        }

        private void OpenPopup()
        {
            popupImage.sprite = _lastClickedRecipe.result.item.icon;
            popupText.text = _lastClickedRecipe.result.item.title;
            popup.SetActive(true);
        }

        public void ClosePopup()
        {
            popup.SetActive(false);
        }

        private void ClearRecipeInfo()
        {
            for (int i = 0; i < _rows.Count; i++)
            {
                _rows[i].SetActive(false);
            }
        }
    }
}
