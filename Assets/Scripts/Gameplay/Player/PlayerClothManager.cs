using System;
using Gameplay.InventorySystem;
using UnityEngine;

namespace Gameplay.Player
{
    public class PlayerClothManager : MonoBehaviour
    {
        public GameObject wearables;

        private void Start()
        {
            Inventory.Instance.EquipmentChanged += OnEquipmentChanged;
            
            OnEquipmentChanged();
        }

        private void OnEquipmentChanged()
        {
            DisableAllClothes();
            WearEquipments();
        }

        private void WearEquipments()
        {
            foreach (var slot in Inventory.Instance.equipmentSlots)
            {
                
                if (slot.item.clothIndex < 0) { continue; }
                Debug.Log(slot.item.clothIndex);
                wearables.transform.GetChild(slot.item.clothIndex).gameObject.SetActive(true);
            }
        }

        private void DisableAllClothes()
        {
            for (int i = 0; i < wearables.transform.childCount; i++)
            {
                wearables.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
