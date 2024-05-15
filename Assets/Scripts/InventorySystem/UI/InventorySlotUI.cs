using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem.UI
{
    public class InventorySlotUI : MonoBehaviour
    {
        public InventorySlot slot;
        public Image frameBackground;
        public Image frameEdge;
        public Image icon;
        public GameObject countObject;

        public void UpdateUI()
        {
            icon.sprite = slot.item.icon;
            countObject.GetComponentInChildren<TextMeshProUGUI>().text = slot.itemCount.ToString();
        }
    }
}
