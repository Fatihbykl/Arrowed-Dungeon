using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopEvents
{
    public static Action<ShopBaseSO> ItemUpgraded;
    public static Action<ShopBaseSO> SkillBought;
    public static Action PurchaseFailed;
}
