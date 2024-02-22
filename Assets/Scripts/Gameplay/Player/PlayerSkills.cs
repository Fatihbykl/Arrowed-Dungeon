using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class PlayerSkills : MonoBehaviour, IDataPersistence
{
    private GameObject forceField;
    private int freezeSkillAmount = 0;
    private int immortalSkillAmount = 0;
    private int destroyerSkillAmount = 0;

    private void OnEnable()
    {
        ShopEvents.SkillBought += OnSkillBought;
    }

    private void OnDisable()
    {
        ShopEvents.SkillBought -= OnSkillBought;
    }

    void Start()
    {
        forceField = transform.GetChild(1).gameObject;
    }

    public void ActivateImmortalSkill()
    {
        if (immortalSkillAmount > 0 && forceField.activeSelf == false)
        {
            StartCoroutine(Immortal());
            immortalSkillAmount--;
        }
    }

    IEnumerator Immortal()
    {
        forceField.SetActive(true);
        yield return new WaitForSeconds(3);
        // add cooldown maybe???
        forceField.SetActive(false);
    }

    public void ActivateFreezeSkill()
    {
        if (freezeSkillAmount > 0)
        {
            GameplayEvents.FreezeSkillActivated.Invoke(0.5f);
            freezeSkillAmount--;
        }
    }

    public void ActivateDestroyerSkill()
    {
        if(destroyerSkillAmount > 0)
        {
            GameplayEvents.DestroyerSkillActivated.Invoke(3);
            destroyerSkillAmount--;
        }
    }

    private void OnSkillBought(ShopBaseSO item)
    {
        if (item.title == ItemTitle.Freeze) { freezeSkillAmount++; }
        else if (item.title == ItemTitle.Immortal) { immortalSkillAmount++; }
        else if (item.title == ItemTitle.Destroyer) { destroyerSkillAmount++; }
    }

    public void LoadData(GameData data)
    {
        freezeSkillAmount = data.freeze;
        immortalSkillAmount = data.immortal;
        destroyerSkillAmount = data.destroyer;
    }

    public void SaveData(GameData data)
    {
        data.freeze = freezeSkillAmount;
        data.immortal = immortalSkillAmount;
        data.destroyer = destroyerSkillAmount;
    }
}
