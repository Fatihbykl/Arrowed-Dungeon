using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTitle { Health, Shield, Speed, Freeze, Immortal, Destroyer }
public enum Category { Upgradeables, Skills }

public abstract class ShopBaseSO : ScriptableObject
{
    public ItemTitle title;
    public string description;
    public Sprite icon;
    public Category category;
    public int cost;

    public abstract string GetTitleInfoText();
    public abstract string GetCost();
    public abstract void BuyItem();
    public abstract float GetCurrentStat();
}
