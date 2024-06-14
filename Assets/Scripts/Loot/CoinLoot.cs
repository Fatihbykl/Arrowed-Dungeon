using System;
using Gameplay.Player;
using UnityEngine;

namespace Loot
{
    public class CoinLoot : LootDrop
    {
        public CoinType coinType;

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player"))
            {
                Coin.Instance.AddCoin(1, coinType);
                Destroy(gameObject);
            }
        }
    }
}
