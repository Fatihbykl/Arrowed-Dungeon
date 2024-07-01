using System;
using DataPersistance.Data.ScriptableObjects;

namespace Events
{
    public static class ShopEvents
    {
        public static Action<ShopBaseSO> ItemUpgraded;
        public static Action<ShopBaseSO> SkillBought;
        public static Action PurchaseFailed;
    }
}
