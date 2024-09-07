using System;
using System.Collections.Generic;
using Gameplay.InventorySystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Player
{
    [Serializable]
    public struct ArrowSlot
    {
        public GameObject arrowPrefab;
        public Item arrowItem;
        [HideInInspector]
        public InventorySlot arrowSlot;
    }
    public class PlayerArrowSwitcher : MonoBehaviour
    {
        public ArrowSlot[] arrows;
        public Image arrowIcon;
        public TextMeshProUGUI arrowCountText;
        
        private List<ArrowSlot> _arrows;
        private int _currentArrowIndex;

        private void Start()
        {
            _arrows = new List<ArrowSlot>();
            GetArrowsFromInventory();
            UpdateUI();
        }

        public GameObject GetCurrentArrowPrefab()
        {
            var currentArrow = _arrows[_currentArrowIndex % _arrows.Count];
            
            Inventory.Instance.RemoveItem(currentArrow.arrowItem, 1);
            if (currentArrow.arrowSlot.itemCount == 0) { _arrows.Remove(currentArrow); }
            UpdateUI();
            
            return currentArrow.arrowPrefab;
        }

        public void SwitchArrow()
        {
            _currentArrowIndex++;
            UpdateUI();
        }
        
        private void GetArrowsFromInventory()
        {
            foreach (var arrow in arrows)
            {
                var a = arrow;
                var slot = Inventory.Instance.GetInventorySlot(arrow.arrowItem);
                if (slot != null)
                {
                    a.arrowSlot = slot;
                    _arrows.Add(a);
                }
            }
        }

        private void UpdateUI()
        {
            var currentArrow = _arrows[_currentArrowIndex % _arrows.Count];
            arrowIcon.sprite = currentArrow.arrowSlot.item.icon;
            arrowCountText.text = currentArrow.arrowSlot.itemCount == -1 ? "\u221e" : currentArrow.arrowSlot.itemCount.ToString();
        }
    }
}
