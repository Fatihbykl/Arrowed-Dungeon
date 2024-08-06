using Gameplay.InventorySystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.CraftSystem
{
    public class CraftInfoRowUI : MonoBehaviour
    {
        public TextMeshProUGUI countText;
        public TextMeshProUGUI titleText;
        public Image iconImage;
        public Image iconFrame;

        public void Init(string title, Color titleColor, Sprite icon, Sprite frame, int requiredMaterialCount, InventorySlot inventoryItem)
        {
            titleText.text = title;
            titleText.color = titleColor;
            iconImage.sprite = icon;
            iconFrame.sprite = frame;
            
            countText.text = requiredMaterialCount + "x";
            if (inventoryItem == null || inventoryItem.itemCount < requiredMaterialCount)
            {
                countText.color = Color.red;
            }
            else
            {
                countText.color = Color.green;
            }
        }
    }
}
