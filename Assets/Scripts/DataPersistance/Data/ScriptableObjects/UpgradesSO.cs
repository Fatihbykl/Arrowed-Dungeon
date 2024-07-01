using Events;
using UnityEngine;

namespace DataPersistance.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Upgradeable Item", menuName = "Shop Items/Upgradeable")]
    public class UpgradesSO : ShopBaseSO
    {
        public int currentLevel;
        public float currentStat;
        public UpgradesSO nextLevel;

        public override void BuyItem()
        {
            UpgradesSO next = this.nextLevel;
            if (next != null)
            {
                this.cost = next.cost;
                this.currentLevel = next.currentLevel;
                this.nextLevel = next.nextLevel;
                this.currentStat = next.currentStat;
                ShopEvents.ItemUpgraded.Invoke(this);
            }
        }

        public override string GetCost()
        {
            var next = nextLevel;
            if (next != null ) 
            { 
                return next.cost.ToString();
            }
            else 
            {
                return "MAX";
            }
        }

        public override float GetCurrentStat()
        {
            return currentStat;
        }

        public override string GetTitleInfoText()
        {
            if (nextLevel != null) 
            {
                return $"{currentLevel} -> {currentLevel + 1}";
            }
            else
            {
                return "Max Lv.";
            }
        
        }
    }
}
