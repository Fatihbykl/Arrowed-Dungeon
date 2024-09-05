using System;
using Gameplay.Managers;
using Gameplay.Player;
using UI.Dynamic_Floating_Text.Scripts;
using UnityEngine;

namespace Gameplay.Loot
{
    public class CoinLoot : LootDrop
    {
        public CoinType coinType;
        public int coinAmount;
        public GameObject particle;

        private void Start()
        {
            Coin.Instance.AddCoin(coinAmount, coinType);
            
            AudioManager.Instance.PlaySoundFXClip(lootCollectSound, transform);
            CreateDynamicText();
            particle.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject, 2f);
        }

        private void CreateDynamicText()
        {
            DynamicTextData data;
            switch (coinType)
            {
                case CoinType.Gem:
                    data = DynamicTextManager.Instance.GemData;
                    break;
                case CoinType.Gold:
                    data = DynamicTextManager.Instance.GoldData;
                    break;
                default:
                    data = DynamicTextManager.Instance.DefaultData;
                    break;
            }
            DynamicTextManager.Instance.CreateText(transform, coinAmount.ToString(), data);
        }
    }
}
