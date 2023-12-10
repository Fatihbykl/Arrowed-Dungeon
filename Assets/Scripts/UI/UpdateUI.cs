using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class UpdateUI : MonoBehaviour
{
    public GameObject pauseObj;
    public GameObject levelPassedObj;
    public GameObject levelFailedObj;
    public GameObject gameManager;
    private GameManager manager;
    private Label healthText, coinText, keyText, brokenArrowText;
    private Button pauseButton, continueButton, mainMenuButton;
    private VisualElement pauseBg, passedBg, failedBg;

    private void OnEnable()
    {
        UIDocument inGameScreenDocument = gameObject.GetComponent<UIDocument>();
        UIDocument pauseDocument = pauseObj.GetComponent<UIDocument>();
        UIDocument passedDocument = levelPassedObj.GetComponent<UIDocument>();
        UIDocument failedDocument = levelFailedObj.GetComponent<UIDocument>();

        healthText = inGameScreenDocument.rootVisualElement.Q("healthText") as Label;
        coinText = inGameScreenDocument.rootVisualElement.Q("coinText") as Label;
        keyText = inGameScreenDocument.rootVisualElement.Q("keyText") as Label;
        brokenArrowText = inGameScreenDocument.rootVisualElement.Q("brokenArrowText") as Label;
        pauseButton = inGameScreenDocument.rootVisualElement.Q("pauseButton") as Button;
        continueButton = pauseDocument.rootVisualElement.Q("continue") as Button;
        mainMenuButton = pauseDocument.rootVisualElement.Q("main-menu") as Button;
        pauseBg = pauseDocument.rootVisualElement.Q("transparent-background");
        passedBg = passedDocument.rootVisualElement.Q("transparent-background");
        failedBg = failedDocument.rootVisualElement.Q("transparent-background");

        pauseButton.RegisterCallback<ClickEvent>(onPauseClicked);
        continueButton.RegisterCallback<ClickEvent>(onContinueClicked);
        mainMenuButton.RegisterCallback<ClickEvent>(onMenuClicked);

        GameplayEvents.ArrowDead += onArrowDead;
        GameplayEvents.KeyCollected += onKeyCollected;
        GameplayEvents.PlayerGetDamaged += onPlayerGetDamaged;
        GameplayEvents.LevelPassed += onLevelPassed;
        GameplayEvents.LevelFailed += onLevelFailed;

        pauseBg.style.display = DisplayStyle.None;
        passedBg.style.display = DisplayStyle.None;
        failedBg.style.display = DisplayStyle.None;
    }

    private void onLevelFailed()
    {
        failedBg.style.display = DisplayStyle.Flex;
    }

    private void onLevelPassed()
    {
        passedBg.style.display = DisplayStyle.Flex;
    }

    private void onMenuClicked(ClickEvent evt)
    {
        // Go to main menu scene
    }

    private void onContinueClicked(ClickEvent evt)
    {
        pauseBg.style.display = DisplayStyle.None;
        Time.timeScale = 1;
    }

    private void onPauseClicked(ClickEvent evt)
    {
        pauseBg.style.display = DisplayStyle.Flex;
        Time.timeScale = 0;
    }

    private void onPlayerGetDamaged(int health)
    {
        healthText.text = health.ToString();
    }

    // make these when on scene loaded
    private void Start()
    {
        manager = gameManager.GetComponent<GameManager>();
        keyText.text = $"{0}/{manager.totalKeyCount}";
        //healthText.text = ;
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
