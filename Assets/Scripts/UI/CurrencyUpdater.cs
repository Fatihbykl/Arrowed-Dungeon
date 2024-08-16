using System;
using Events;
using Gameplay.Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CurrencyUpdater : MonoBehaviour
    {
        public TextMeshProUGUI coinText;
        public TextMeshProUGUI gemText;

        private void Start()
        {
            EventManager.StartListening(EventStrings.CurrencyUpdated, OnCurrencyUpdated);
            
            OnCurrencyUpdated();
        }

        private void OnCurrencyUpdated()
        {
            coinText.text = Coin.Instance.Gold.ToString();
            gemText.text = Coin.Instance.Gem.ToString();
        }

        private void OnDestroy()
        {
            EventManager.StopListening(EventStrings.CurrencyUpdated, OnCurrencyUpdated);
        }
    }
}
