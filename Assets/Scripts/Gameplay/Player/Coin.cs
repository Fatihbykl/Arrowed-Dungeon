using System;
using TMPro;
using UnityEngine;

namespace Gameplay.Player
{
    public enum CoinType { Gold, Gem }
    [Serializable]
    public struct Cost { public int amount; public CoinType coinType; }
    
    public class Coin : MonoBehaviour
    {
        public int gold, gem;
        public TextMeshProUGUI goldText, gemText;

        public static Coin Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Found more than one Inventory in the scene.");
                Destroy(this);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            goldText.text = gold.ToString();
            gemText.text = gem.ToString();
        }

        public void AddCoin(int amount, CoinType type)
        {
            switch (type)
            {
                case CoinType.Gold:
                    gold += amount;
                    break;
                case CoinType.Gem:
                    gem += amount;
                    break;
            }
        }

        public bool SpendCoin(int amount, CoinType type)
        {
            switch (type)
            {
                case CoinType.Gold:
                    return SpendGold(amount);
                case CoinType.Gem:
                    return SpendGem(amount);
                default:
                    return false;
            }
        }

        private bool SpendGold(int amount)
        {
            if (gold < amount) { return false; }
            
            gold -= amount;
            return true;
        }
        
        private bool SpendGem(int amount)
        {
            if (gem < amount) { return false; }
            
            gem -= amount;
            return true;
        }
    }
}
