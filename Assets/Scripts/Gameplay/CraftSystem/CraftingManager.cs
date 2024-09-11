using System;
using System.Collections.Generic;
using Events;
using Gameplay.CraftSystem.DynamicScroll;
using Gameplay.InventorySystem;
using Gameplay.Player;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.CraftSystem
{
    public class CraftingManager : MonoBehaviour
    {
        public RarityColorsFramesInfo rarityInfo;
        
        [Header("Recipe Details")]
        public GameObject recipeInfoParent;
        public GameObject recipeInfoPrefab;
        public int recipeInfoRowCount;
        public TextMeshProUGUI craftButtonCostText;
        public TextMeshProUGUI craftChanceText;

        [Header("Upgrade Craft Chance")] 
        public Item chanceItem;
        public int addedChancePercent = 5;

        [Header("After Craft Popup")]
        public GameObject successPopup;
        public GameObject failurePopup;
        public Image popupImage;
        public TextMeshProUGUI popupText;
        
        private List<GameObject> _rows;
        private CraftingRecipe _lastClickedRecipe;
        private int _addedChance;

        private void Start()
        {
            EventManager.StartListening(EventStrings.RecipeClicked, OnRecipeClicked);

            _addedChance = 0;
            _rows = new List<GameObject>();
            
            InstantiateSlots();
        }

        private void InstantiateSlots()
        {
            for (int i = 0; i < recipeInfoRowCount; i++)
            {
                var row = Instantiate(recipeInfoPrefab, recipeInfoParent.transform);
                row.SetActive(false);

                _rows.Add(row);
            }
        }

        public void OnRecipeClicked()
        {
            var slot = (RecipeData)EventManager.GetSender(EventStrings.RecipeClicked);
            
            _lastClickedRecipe = slot.recipe;
            ClearRecipeInfo();
            UpdateCraftChance();
            craftButtonCostText.text = slot.recipe.craftCost.amount.ToString();
            
            for (int i = 0; i < slot.recipe.materials.Count; i++){

                var title = slot.recipe.materials[i].item.title;
                var icon = slot.recipe.materials[i].item.icon;
                var material = slot.recipe.materials[i];
                var frame = rarityInfo.rarityFrames[(int)material.item.itemRarity];
                var color = rarityInfo.rarityColors[(int)material.item.itemRarity];
                var inventoryItem = Inventory.Instance.GetInventorySlot(material.item);
                
                _rows[i].GetComponent<CraftInfoRowUI>().Init(title, color, icon, frame, material.itemCount, inventoryItem);
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
                var extraLuck = _addedChance * addedChancePercent;
                var craftResult = _lastClickedRecipe.Craft(extraLuck, chanceItem, _addedChance);
                
                switch (craftResult)
                {
                    case CraftStates.Success:
                        OpenPopup();
                        break;
                    case CraftStates.Failure:
                        failurePopup.SetActive(true);
                        break;
                }
            }
            else
            {
                Debug.LogWarning("Not enough coin!");
            }
        }

        public void AddChance()
        {
            var item = Inventory.Instance.GetInventorySlot(chanceItem);
            if (item == null || _addedChance >= item.itemCount) { return; }

            _addedChance++;
            UpdateCraftChance();
        }

        private void UpdateCraftChance()
        {
            var currentChance = _lastClickedRecipe.craftChance + _addedChance * addedChancePercent;
            var colorLerp = Color.Lerp(Color.red, Color.green, currentChance / 100f);
            craftChanceText.text = $"Craft Chance: {currentChance.ToString()}%";
            craftChanceText.color = colorLerp;
        }

        private void OpenPopup()
        {
            popupImage.sprite = _lastClickedRecipe.result.item.icon;
            popupText.text = _lastClickedRecipe.result.item.title;
            popupText.color = rarityInfo.rarityColors[(int)_lastClickedRecipe.result.item.itemRarity];
            successPopup.SetActive(true);
        }

        public void ClosePopup()
        {
            successPopup.SetActive(false);
            failurePopup.SetActive(false);
        }

        private void ClearRecipeInfo()
        {
            _addedChance = 0;
            for (int i = 0; i < _rows.Count; i++)
            {
                _rows[i].SetActive(false);
            }
        }

        private void OnDestroy()
        {
            EventManager.StartListening(EventStrings.RecipeClicked, OnRecipeClicked);
        }
    }
}
