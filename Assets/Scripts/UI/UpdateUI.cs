using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using DG.Tweening;
using Events;
using Managers;

public class UpdateUI : MonoBehaviour
{
    public GameObject pauseObj;
    public GameObject levelPassedObj;
    public GameObject levelFailedObj;

    private Label healthText, coinText, keyText, brokenArrowText;
    private Button pauseButton, continueButton, mainMenuButton;
    private VisualElement pauseBg, passedBg, failedBg, keyIcon, key;

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
        keyIcon = inGameScreenDocument.rootVisualElement.Q("keyIcon");
        keyIcon = inGameScreenDocument.rootVisualElement.Q("key");
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
        GameplayEvents.KeyAnimationFinished += OnKeyAnimationFinished;
        GameplayEvents.PlayerGetDamaged += onPlayerGetDamaged;
        GameplayEvents.LevelPassed += onLevelPassed;
        GameplayEvents.LevelFailed += onLevelFailed;
        SceneManager.sceneLoaded += OnSceneLoaded;

        pauseBg.style.display = DisplayStyle.None;
        passedBg.style.display = DisplayStyle.None;
        failedBg.style.display = DisplayStyle.None;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        keyText.text = $"{0}/{GameManager.instance.totalKeyCount}";
        coinText.text = GameManager.instance.collectedKeyCount.ToString();
        healthText.text = GameManager.instance.playerBaseHealth.ToString();
    }

    private void OnDisable()
    {
        GameplayEvents.ArrowDead -= onArrowDead;
        GameplayEvents.KeyAnimationFinished -= OnKeyAnimationFinished;
        GameplayEvents.PlayerGetDamaged -= onPlayerGetDamaged;
        GameplayEvents.LevelPassed -= onLevelPassed;
        GameplayEvents.LevelFailed -= onLevelFailed;
        SceneManager.sceneLoaded -= OnSceneLoaded;
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

    private void OnKeyAnimationFinished()
    {
        keyText.text = $"{GameManager.instance.collectedKeyCount.ToString()}/{GameManager.instance.totalKeyCount.ToString()}";
        
        DotweenAnimations.DoPunchAnimation(keyText, new Vector3(0.3f, 0.3f, 0.3f), 0.2f, Ease.InOutElastic);
        DotweenAnimations.DoPunchAnimation(keyIcon, new Vector3(0.3f, 0.3f, 0.3f), 0.2f, Ease.InOutElastic);
        DotweenAnimations.DoPunchAnimation(key, new Vector3(0.3f, 0.3f, 0.3f), 0.2f, Ease.InOutElastic);
    }

    public void onArrowDead(string arrowType, int arrowReward)
    {
        var coins = GameManager.instance.currentLevelCoin.ToString();
        var brokenArrows = GameManager.instance.currentLevelBrokenArrows.ToString();

        coinText.text = coins;
        brokenArrowText.text = brokenArrows;
    }
}