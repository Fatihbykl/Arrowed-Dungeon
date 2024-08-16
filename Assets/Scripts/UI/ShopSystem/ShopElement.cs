using System;
using Coffee.UIEffects;
using Events;
using Gameplay.InventorySystem;
using Gameplay.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShopSystem
{
    public class ShopElement : MonoBehaviour
    {
        public Image icon;
        public TextMeshProUGUI title;
        public TextMeshProUGUI description;

        [Header("Button")]
        public Image buttonBackground;
        public Image buttonIcon;
        public TextMeshProUGUI buttonCost;
        public Sprite goldSprite;
        public Sprite goldButtonBgSprite;
        public Sprite gemSprite;
        public Sprite gemButtonBgSprite;

        public Item ShopItem { get; private set; }
        public RarityColorsFramesInfo ColorInfo { get; private set; }

        public void Init(Item item, RarityColorsFramesInfo colorInfo)
        {
            ShopItem = item;
            ColorInfo = colorInfo;
            
            icon.sprite = item.icon;
            title.text = item.title;
            description.text = item.description;
            buttonCost.text = item.cost.amount.ToString();
            GetComponent<UIShadow>().effectColor = colorInfo.rarityColors[(int)item.itemRarity];
            
            if (item.cost.coinType == CoinType.Gold)
            {
                buttonBackground.sprite = goldButtonBgSprite;
                buttonIcon.sprite = goldSprite;
            }
            else
            {
                buttonBackground.sprite = gemButtonBgSprite;
                buttonIcon.sprite = gemSprite;
            }
        }

        public void BuyButtonClicked()
        {
            EventManager.EmitEvent(EventStrings.ShopBuyButtonClicked, gameObject);
        }
    }
}
