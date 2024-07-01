using Gameplay.Managers;
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
                Coin.Instance.AddCoin(1, coinType);
                AudioManager.Instance.PlaySoundFXClip(lootCollectSound, transform);
                Destroy(gameObject);
            }
        }
    }
}
