using System;

namespace StatSystem
{
    [Serializable]
    public class FloatStat : CharacterStat<float>
    {
        public override float Value
        {
            get
            {
                if (isDirty)
                {
                    value = CalculateFinalValue();
                    isDirty = false;
                }

                return (float)Math.Round(value, 2);
            }
        }

        public override float BaseValue
        {
            get => (float)Math.Round(baseValue, 2);
            set => baseValue = value;
        }
    }
}