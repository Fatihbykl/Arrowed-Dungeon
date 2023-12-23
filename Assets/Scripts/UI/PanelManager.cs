using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class PanelManager : MonoBehaviour
{
    [SerializeField]
    private ManageScenes manageScenes;

    [SerializeField]
    private GameObject mainMenu, settings, charCustomization;
    
    [SerializeField]
    private Color[] colors;

    [SerializeField]
    Material hairMaterial, eyesMaterial, eyebrowsMaterial, tshirtMaterial, pantsMaterial, shoesMaterial;

    private UIDocument menuDocument;
    private UIDocument settingsDocument;
    private UIDocument characterDocument;

    private Button _playButton, _shopButton, _customizationButton, _settingsButton, _quitButton;
    private VisualElement backgroundMenu, backgroundSettings, backgroundCustomization;
    
    private void OnEnable()
    {
        menuDocument = mainMenu.GetComponent<UIDocument>();
        settingsDocument = settings.GetComponent<UIDocument>();
        characterDocument = charCustomization.GetComponent<UIDocument>();

        backgroundMenu = menuDocument.rootVisualElement.Q("Background");
        backgroundSettings = settingsDocument.rootVisualElement.Q("Background");
        backgroundCustomization = characterDocument.rootVisualElement.Q("BackgroundChar");

        // Menu screen buttons
        _playButton = menuDocument.rootVisualElement.Q("playButton") as Button;
        _shopButton = menuDocument.rootVisualElement.Q("shopButton") as Button;
        _customizationButton = menuDocument.rootVisualElement.Q("customizationButton") as Button;
        _settingsButton = menuDocument.rootVisualElement.Q("settingsButton") as Button;
        _quitButton = menuDocument.rootVisualElement.Q("quitButton") as Button;

        // Settings screen buttons
        Button _settingsBackButton = settingsDocument.rootVisualElement.Q("backButton") as Button;
        SliderInt _musicVolume = settingsDocument.rootVisualElement.Q("musicSlider") as SliderInt;
        SliderInt _sfxVolume = settingsDocument.rootVisualElement.Q("sfxSlider") as SliderInt;

        // Customization screen buttons
        Button _customizationBackButton = characterDocument.rootVisualElement.Q("backButton") as Button;
        GroupBox _hairRadio = characterDocument.rootVisualElement.Q("hairRadioGroup") as GroupBox;
        GroupBox _eyesRadio = characterDocument.rootVisualElement.Q("eyesRadioGroup") as GroupBox;
        GroupBox _eyebrowsRadio = characterDocument.rootVisualElement.Q("eyebrowsRadioGroup") as GroupBox;
        GroupBox _tshirtRadio = characterDocument.rootVisualElement.Q("tshirtRadioGroup") as GroupBox;
        GroupBox _pantsRadio = characterDocument.rootVisualElement.Q("pantsRadioGroup") as GroupBox;
        GroupBox _shoesRadio = characterDocument.rootVisualElement.Q("shoesRadioGroup") as GroupBox;
        
        
        // Menu screen callbacks
        _customizationButton.RegisterCallback<ClickEvent>(OpenCustomizationScreen);
        _settingsButton.RegisterCallback<ClickEvent>(OpenSettingsScreen);
        _playButton.RegisterCallback<ClickEvent>(OpenLevelsScene);
        _shopButton.RegisterCallback<ClickEvent>(OpenShopScene);

        // Settings screen callbacks
        _settingsBackButton.RegisterCallback<ClickEvent>(GoMainMenu);
        _musicVolume.RegisterValueChangedCallback(OnMusicSliderChanged);
        _sfxVolume.RegisterValueChangedCallback(OnSfxSliderChanged);

        // Customization screen callbacks
        _customizationBackButton.RegisterCallback<ClickEvent>(GoMainMenu);
        _hairRadio.RegisterCallback<ChangeEvent<bool>>(evt => {
            var radio = evt.target as RadioButton;
            if (radio.value) { hairMaterial.color = colors[radio.tabIndex]; }
        });
        _eyesRadio.RegisterCallback<ChangeEvent<bool>>(evt => {
            var radio = evt.target as RadioButton;
            if (radio.value) { eyesMaterial.color = colors[radio.tabIndex]; }
        });
        _eyebrowsRadio.RegisterCallback<ChangeEvent<bool>>(evt => {
            var radio = evt.target as RadioButton;
            if (radio.value) { eyebrowsMaterial.color = colors[radio.tabIndex]; }
        });
        _tshirtRadio.RegisterCallback<ChangeEvent<bool>>(evt => {
            var radio = evt.target as RadioButton;
            if (radio.value) { tshirtMaterial.color = colors[radio.tabIndex]; }
        });
        _pantsRadio.RegisterCallback<ChangeEvent<bool>>(evt => {
            var radio = evt.target as RadioButton;
            if (radio.value) { pantsMaterial.color = colors[radio.tabIndex]; }
        });
        _shoesRadio.RegisterCallback<ChangeEvent<bool>>(evt => {
            var radio = evt.target as RadioButton;
            if (radio.value) { shoesMaterial.color = colors[radio.tabIndex]; }
        });
        
        menuDocument.rootVisualElement.transform.scale = Vector3.zero;
        settingsDocument.rootVisualElement.transform.scale = Vector3.zero;
        characterDocument.rootVisualElement.transform.scale = Vector3.zero;
        
        MainMenuAnimation();
    }

    private async void MainMenuAnimation()
    {
        float wait = 0.3f;
        DotweenAnimations.DoScaleFromZeroAnimation(menuDocument.rootVisualElement);
        await UniTask.WaitForSeconds(wait);
        DotweenAnimations.DoScaleFromZeroAnimation(_playButton);
        await UniTask.WaitForSeconds(wait);
        DotweenAnimations.DoScaleFromZeroAnimation(_shopButton);
        await UniTask.WaitForSeconds(wait);
        DotweenAnimations.DoScaleFromZeroAnimation(_customizationButton);
        await UniTask.WaitForSeconds(wait);
        DotweenAnimations.DoScaleFromZeroAnimation(_settingsButton);
        await UniTask.WaitForSeconds(wait);
        DotweenAnimations.DoScaleFromZeroAnimation(_quitButton);
    }

    private void OnSfxSliderChanged(ChangeEvent<int> value)
    {
        Debug.Log(value.previousValue);
        Debug.Log(value.newValue);
    }

    private void OnMusicSliderChanged(ChangeEvent<int> value)
    {
        Debug.Log(value.previousValue);
        Debug.Log(value.newValue);
    }

    private void OpenShopScene(ClickEvent evt)
    {
        manageScenes.LoadScene("Shop");
    }

    private void OpenLevelsScene(ClickEvent evt)
    {
        manageScenes.LoadScene("Levels");
    }

    private void OpenCustomizationScreen(ClickEvent evt)
    {
        menuDocument.rootVisualElement.transform.scale = Vector3.zero;
        settingsDocument.rootVisualElement.transform.scale = Vector3.zero;
        DotweenAnimations.DoScaleFromZeroAnimation(characterDocument.rootVisualElement);
    }

    private void OpenSettingsScreen(ClickEvent evt)
    {
        menuDocument.rootVisualElement.transform.scale = Vector3.zero;
        characterDocument.rootVisualElement.transform.scale = Vector3.zero;
        DotweenAnimations.DoScaleFromZeroAnimation(settingsDocument.rootVisualElement);
    }

    private void GoMainMenu(ClickEvent evt)
    {
        Debug.Log("back");
        characterDocument.rootVisualElement.transform.scale = Vector3.zero;
        settingsDocument.rootVisualElement.transform.scale = Vector3.zero;
        DotweenAnimations.DoScaleFromZeroAnimation(menuDocument.rootVisualElement);
    }
}
