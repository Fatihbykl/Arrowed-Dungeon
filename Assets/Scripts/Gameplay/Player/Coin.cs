using System;
using DataPersistance;
using DataPersistance.Data;
using Events;
using TMPro;
using UnityEngine;

namespace Gameplay.Player
{
    public enum CoinType { Gold, Gem }
    [Serializable]
    public struct Cost { public int amount; public CoinType coinType; }
    
    public class Coin : MonoBehaviour, IDataPersistence
    {
        public int Gold { get; private set; }
        public int Gem { get; private set; }
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
            Gold = 200;
            Gem = 200;
        }

        public void AddCoin(int amount, CoinType type)
        {
            if (amount < 0) { Debug.LogError("Amount must be greater than zero!"); return; }
            switch (type)
            {
                case CoinType.Gold:
                    Gold += amount;
                    break;
                case CoinType.Gem:
                    Gem += amount;
                    break;
            }
            EventManager.EmitEvent(EventStrings.CurrencyUpdated);
        }

        public bool SpendCoin(int amount, CoinType type)
        {
            if (amount < 0) { Debug.LogError("Amount must be greater than zero!"); return false; }

            return type switch
            {
                CoinType.Gold => SpendGold(amount),
                CoinType.Gem => SpendGem(amount),
                _ => false
            };
        }

        public int GetCoin(CoinType type)
        {
            return type switch
            {
                CoinType.Gem => Gem,
                CoinType.Gold => Gold,
                _ => 0
            };
        }

        private bool SpendGold(int amount)
        {
            if (Gold < amount) { return false; }
            
            Gold -= amount;
            EventManager.EmitEvent(EventStrings.CurrencyUpdated);
            
            return true;
        }
        
        private bool SpendGem(int amount)
        {
            if (Gem < amount) { return false; }
            
            Gem -= amount;
            EventManager.EmitEvent(EventStrings.CurrencyUpdated);
            
            return true;
        }

        public bool IsLoaded { get; set; }

        public void LoadData(GameData data)
        {
            Gold = data.coinData.gold;
            Gem = data.coinData.gem;
        }

        public void SaveData(GameData data)
        {
            data.coinData.gold = Gold;
            data.coinData.gem = Gem;
        }
    }
}
