using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class UpdateUI : MonoBehaviour
{
    public GameObject gameManager;
    private GameManager manager;
    private Label healthText, coinText, keyText, brokenArrowText;

    private void OnEnable()
    {
        var inGameScreenDocument = gameObject.GetComponent<UIDocument>();

        healthText = inGameScreenDocument.rootVisualElement.Q("healthText") as Label;
        coinText = inGameScreenDocument.rootVisualElement.Q("coinText") as Label;
        keyText = inGameScreenDocument.rootVisualElement.Q("keyText") as Label;
        brokenArrowText = inGameScreenDocument.rootVisualElement.Q("brokenArrowText") as Label;

        GameplayEvents.ArrowDead += onArrowDead;
        GameplayEvents.KeyCollected += onKeyCollected;
        GameplayEvents.PlayerGetDamaged += onPlayerGetDamaged;
    }

    private void onPlayerGetDamaged(int health)
    {
        healthText.text = health.ToString();
    }

    private void Start()
    {
        manager = gameManager.GetComponent<GameManager>();
        keyText.text = $"{0}/{manager.totalKeyCount}";
    }

    private void onKeyCollected(int collectedKeyCount, int totalKeyCount)
    {
        keyText.text = $"{collectedKeyCount.ToString()}/{totalKeyCount.ToString()}";
    }

    public void onArrowDead(string arrowType, int arrowReward)
    {
        var coins = manager.currentLevelCoin.ToString();
        var brokenArrows = manager.currentLevelBrokenArrows.ToString();
        
        coinText.text = coins;
        brokenArrowText.text = brokenArrows;
    }
}
