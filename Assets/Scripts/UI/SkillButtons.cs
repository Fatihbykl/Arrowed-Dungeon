using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillButtons : MonoBehaviour
{
    [SerializeField]
    private PlayerSkills player;
    private Button immortal, freeze, destroyer;

    private void OnEnable()
    {
        UIDocument skillsDocument = GetComponent<UIDocument>();

        immortal = skillsDocument.rootVisualElement.Q<Button>("immortalSkill");
        freeze = skillsDocument.rootVisualElement.Q<Button>("freezeSkill");
        destroyer = skillsDocument.rootVisualElement.Q<Button>("destroyerSkill");

        immortal.RegisterCallback<ClickEvent>(onImmortalClicked);
        freeze.RegisterCallback<ClickEvent>(onFreezeClicked);
        destroyer.RegisterCallback<ClickEvent>(onDestroyerClicked);
    }

    private void onDestroyerClicked(ClickEvent evt)
    {
        player.ActivateDestroyerSkill();
    }

    private void onFreezeClicked(ClickEvent evt)
    {
        player.ActivateFreezeSkill();
    }

    private void onImmortalClicked(ClickEvent evt)
    {
        player.ActivateImmortalSkill();
    }
}
