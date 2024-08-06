using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.CraftSystem
{
    public class CraftingSlotUI : MonoBehaviour
    {
        public Image icon;
        public Image iconFrame;
        public TextMeshProUGUI itemName;
        
        [HideInInspector]
        public CraftingRecipe recipe;

        public void Init(RarityColorsFramesInfo rarityInfo)
        {
            icon.sprite = recipe.result.item.icon;
            iconFrame.sprite = rarityInfo.rarityFrames[(int)recipe.result.item.itemRarity];
            itemName.text = recipe.result.item.title;
        }

        public void OnRecipeClicked()
        {
            CraftingManager.RecipeClicked?.Invoke(this);
        }
    }
}
