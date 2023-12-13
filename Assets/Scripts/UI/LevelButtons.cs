using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelButtons : MonoBehaviour
{
    [SerializeField]
    private ManageScenes manageScenes;
    private List<Button> buttons;

    private void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();

        buttons = document.rootVisualElement.Query<Button>(className: "level-button").ToList();

        foreach (Button button in buttons)
        {
            button.RegisterCallback<ClickEvent>(OnLevelButtonClicked);
        }
    }

    private void OnDisable()
    {
        foreach (Button button in buttons)
        {
            button.UnregisterCallback<ClickEvent>(OnLevelButtonClicked);
        }
    }

    private void OnLevelButtonClicked(ClickEvent evt)
    {
        var targetButton = evt.target as Button;
        manageScenes.LoadScene($"Level_{targetButton.text}");
    }
}
