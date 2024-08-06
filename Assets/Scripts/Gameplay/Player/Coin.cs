using System;
using Events;
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
        public static Coin Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Found more than one Coin in the scene.");
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void AddCoin(int amount, CoinType type)
        {
            if (amount < 0) { Debug.LogError("Amount must be greater than zero!"); return; }
            switch (type)
            {
                case CoinType.Gold:
                    gold += amount;
                    break;
                case CoinType.Gem:
                    gem += amount;
                    break;
            }
            EventManager.EmitEvent(EventStrings.CurrencyUpdated);
        }

        public bool SpendCoin(int amount, CoinType type)
        {
            if (amount < 0) { Debug.LogError("Amount must be greater than zero!"); return false; }
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
            EventManager.EmitEvent(EventStrings.CurrencyUpdated);
            
            return true;
        }
        
        private bool SpendGem(int amount)
        {
            if (gem < amount) { return false; }
            
            gem -= amount;
            EventManager.EmitEvent(EventStrings.CurrencyUpdated);
            
            return true;
        }
    }
}
