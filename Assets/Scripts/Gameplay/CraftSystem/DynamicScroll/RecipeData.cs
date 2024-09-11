using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.CraftSystem.DynamicScroll
{
    [Serializable]
    public class RecipeData
    {
        public CraftingRecipe recipe;
        public RarityColorsFramesInfo rarityInfo;

        public RecipeData(CraftingRecipe recipe, RarityColorsFramesInfo rarityInfo)
        {
            this.recipe = recipe;
            this.rarityInfo = rarityInfo;
        }
    }
}
