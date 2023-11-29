using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu, settings, charCustomization;
    
    [SerializeField]
    private Color[] colors;

    [SerializeField]
    Material hairMaterial, eyesMaterial, eyebrowsMaterial, tshirtMaterial, pantsMaterial, shoesMaterial;

    private UIDocument menuDocument;
    private UIDocument settingsDocument;
    private UIDocument characterDocument;

    private void OnEnable()
    {
        menuDocument = mainMenu.GetComponent<UIDocument>();
        settingsDocument = settings.GetComponent<UIDocument>();
        characterDocument = charCustomization.GetComponent<UIDocument>();

        // Menu screen buttons
        Button _playButton = menuDocument.rootVisualElement.Q("playButton") as Button;
        Button _shopButton = menuDocument.rootVisualElement.Q("shopButton") as Button;
        Button _customizationButton = menuDocument.rootVisualElement.Q("customizationButton") as Button;
        Button _settingsButton = menuDocument.rootVisualElement.Q("settingsButton") as Button;
        Button _quitButton = menuDocument.rootVisualElement.Q("quitButton") as Button;

        // Settings screen buttons
        Button _settingsBackButton = settingsDocument.rootVisualElement.Q("backButton") as Button;

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

        // Settings screen callbacks
        _settingsBackButton.RegisterCallback<ClickEvent>(GoMainMenu);

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

        characterDocument.rootVisualElement.style.display = DisplayStyle.None;
        characterDocument.rootVisualElement.style.display = DisplayStyle.None;
        settingsDocument.rootVisualElement.style.display = DisplayStyle.None;
    }

    private void OpenCustomizationScreen(ClickEvent evt)
    {
        menuDocument.rootVisualElement.style.display = DisplayStyle.None;
        settingsDocument.rootVisualElement.style.display = DisplayStyle.None;
        characterDocument.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    private void OpenSettingsScreen(ClickEvent evt)
    {
        menuDocument.rootVisualElement.style.display = DisplayStyle.None;
        settingsDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        characterDocument.rootVisualElement.style.display = DisplayStyle.None;
    }

    private void GoMainMenu(ClickEvent evt)
    {
        menuDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        settingsDocument.rootVisualElement.style.display = DisplayStyle.None;
        characterDocument.rootVisualElement.style.display = DisplayStyle.None;
    }
}
