using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CraftSystem
{
    public class CraftingSlotUI : MonoBehaviour
    {
        public Image icon;
        public Image iconFrame;
        public TextMeshProUGUI itemName;
        public GameObject infoScreen;
        
        //[HideInInspector]
        public CraftingRecipe recipe;

        public void Init()
        {
            icon.sprite = recipe.result.item.icon;
            // iconFrame
            itemName.text = recipe.result.item.title;
        }

        public void OnButtonClicked()
        {
            
        }
    }
}
