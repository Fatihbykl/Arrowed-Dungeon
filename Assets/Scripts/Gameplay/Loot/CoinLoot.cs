using System;
using Gameplay.Managers;
using Gameplay.Player;
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
            particle.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject, 2f);
        }
        
    }
}
