using System;
using Coffee.UIEffects;
using NaughtyAttributes;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameplay.InventorySystem.UI
{
    public enum SlotType
    {
        EquipmentSlot,
        InventorySlot
    }
    public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
    {
        public InventorySlot slot;
        public SlotType slotType;
        public Image frameBackground;
        public Image icon;
        public GameObject countObject;

        private RarityColorsFramesInfo _rarityInfo;
        private TextMeshProUGUI _countText;
        private UIEffect _effect;

        private void Start()
        {
            _countText = countObject.GetComponentInChildren<TextMeshProUGUI>();
            _effect = GetComponentInChildren<UIEffect>();
        }

        public void Init(InventorySlot inventorySlot, RarityColorsFramesInfo info)
        {
            slot = inventorySlot;
            _rarityInfo = info;
            icon.sprite = slot.item.icon;
            frameBackground.sprite = _rarityInfo.rarityFrames[(int)slot.item.itemRarity];
            countObject.GetComponentInChildren<TextMeshProUGUI>().text = slot.itemCount.ToString();
            
            if (slotType == SlotType.EquipmentSlot)
            {
                countObject.SetActive(false);
                
                if (slot.item.id == "dummy")
                {
                    frameBackground.color = new Color(51 / 255f, 36 / 255f, 21 / 255f);
                    icon.color = new Color(51 / 255f, 36 / 255f, 21 / 255f);
                    _effect.enabled = true;
                }
                else
                {
                    frameBackground.color = Color.white;
                    icon.color = Color.white;
                    _effect.enabled = false;
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (slot.item.id == "dummy") { return; }
            InventoryManagerUI.OpenEquipPopup?.Invoke(slot, slotType);
        }
    }
}