using System;

namespace Gameplay.StatSystem
{
    [Serializable]
    public class IntegerStat : CharacterStat<int>
    {
        public override int Value
        {
            get
            {
                if (isDirty)
                {
                    value = CalculateFinalValue();
                    isDirty = false;
                }

                var tempValue = (int)Math.Round(value, 0);
                if (useUpperBound && tempValue > upperBound) { tempValue = (int)upperBound; }
                if (useLowerBound && tempValue < lowerBound) { tempValue = (int)lowerBound; }

                return tempValue;
            }
        }

        public override int BaseValue
        {
            get => (int)Math.Round(baseValue, 0);
            set => baseValue = value;
        }
    }
}