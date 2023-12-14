using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Item", menuName = "Shop Items/Skill")]
public class SkillsSO : ShopBaseSO
{
    public override void BuyItem()
    {
        ShopEvents.SkillBought.Invoke(this);
    }

    public override string GetCost()
    {
        return this.cost.ToString();
    }

    public override float GetCurrentStat()
    {
        return 0;
    }

    public override string GetTitleInfoText()
    {
        return "";
    }
}
