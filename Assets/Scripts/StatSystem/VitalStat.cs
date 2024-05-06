using System;

namespace StatSystem
{
    [Serializable]
    public class VitalStat : CharacterStat<int>
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
                if (tempValue > baseValue) { RemoveAllModifiers(); }

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
