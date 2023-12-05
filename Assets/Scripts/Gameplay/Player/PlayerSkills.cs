using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    private GameObject forceField;
    private bool isAnySkillActive = false;

    void Start()
    {
        forceField = transform.GetChild(1).gameObject;
    }

    public void ActivateImmortalSkill()
    {
        if (isAnySkillActive) { return; }
        // check if user has this skill
        StartCoroutine(Immortal());
    }

    IEnumerator Immortal()
    {
        forceField.SetActive(true);
        yield return new WaitForSeconds(3); 
        forceField.SetActive(false);
        // add cooldown
    }

    public void ActivateFreezeSkill()
    {
        // check if user has this skill
        GameplayEvents.FreezeSkillActivated.Invoke(0.5f);
        // add cooldown
    }

    public void ActivateDestroyerSkill()
    {
        // check if user has this skill
        GameplayEvents.DestroyerSkillActivated.Invoke(3);
        // add cooldown
    }
}
