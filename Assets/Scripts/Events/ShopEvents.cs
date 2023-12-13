using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopEvents
{
    public static Action<ShopSystem.ItemData.ItemTitle, float> ItemUpgraded;
    public static Action<ShopSystem.ItemData.ItemTitle> SkillBought;
    public static Action PurchaseFailed;
}
