using Gameplay.Player;
using UnityEngine;

namespace Gameplay.Loot
{
    public class CoinLoot : LootDrop
    {
        public CoinType coinType;

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.CompareTag("Player"))
            {
                Debug.Log(other.collider.gameObject.name);
                Coin.Instance.AddCoin(1, coinType);
                Destroy(gameObject);
            }
        }
    }
}
