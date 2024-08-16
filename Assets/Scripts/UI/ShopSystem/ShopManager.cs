using System;
using Events;
using Gameplay.InventorySystem;
using Gameplay.Player;
using TMPro;
using UnityEngine;

namespace UI.ShopSystem
{
    [Serializable]
    public class SubCategory
    {
        public GameObject subCategoryParent;
        public Item[] items;
    }
    
    [Serializable]
    public class CategoryContent
    {
        public GameObject categoryParent;
        public SubCategory[] subCategories;
    }
    
    public class ShopManager : MonoBehaviour
    {
        public ShopElement shopElementPrefab;
        public RarityColorsFramesInfo info;
        public CategoryContent[] tabs;

        [Header("Popup Settings")] 
        public GameObject popupBackground;
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;

        private Item _lastItem;
        
        private void Start()
        {
            EventManager.StartListening(EventStrings.ShopBuyButtonClicked, OnBuyButtonClicked);
            
            InitializeShopItems();
        }

        private void OnDestroy()
        {
            EventManager.StopListening(EventStrings.ShopBuyButtonClicked, OnBuyButtonClicked);
        }

        private void OnBuyButtonClicked()
        {
            var sender = (GameObject)EventManager.GetSender(EventStrings.ShopBuyButtonClicked);
            _lastItem = sender.GetComponent<ShopElement>().ShopItem;

            var isCoinEnough = Coin.Instance.GetCoin(_lastItem.cost.coinType) > _lastItem.cost.amount;
            if (isCoinEnough)
            {
                OpenPopup();
            }
            else
            {
                // coin is not enough to purchase this item
            }

        }

        public void PurchaseItem()
        {
            var isPurchaseSuccessful = Coin.Instance.SpendCoin(_lastItem.cost.amount, _lastItem.cost.coinType);
            if (isPurchaseSuccessful) { Inventory.Instance.AddItem(_lastItem, 1); }
            
            ClosePopup();
        }

        private void OpenPopup()
        {
            popupBackground.SetActive(true);
            title.text = _lastItem.title;
            title.color = info.rarityColors[(int)_lastItem.itemRarity];
            description.text = _lastItem.cost.coinType switch
            {
                CoinType.Gem => $"Buy this item for <#008080ff> {_lastItem.cost.amount} gem</color>?",
                CoinType.Gold =>  $"Buy this item for <#ffff00ff> {_lastItem.cost.amount} gold</color>?",
                _ => ""
            };
        }

        public void ClosePopup()
        {
            popupBackground.SetActive(false);
        }

        private void InitializeShopItems()
        {
            foreach (var tab in tabs)
            {
                foreach (var subCategory in tab.subCategories)
                {
                    foreach (var item in subCategory.items)
                    {
                        var shopElement = Instantiate(shopElementPrefab, subCategory.subCategoryParent.transform);
                        shopElement.Init(item, info);
                    }
                }
            }
        }
    }
}
