namespace StatSystem
{
	public enum StatModType
	{
		Flat = 100,
		PercentAdd = 200,
		PercentMult = 300,
	}

	public enum TargetStat
	{
		None,
		MaxHealth,
		Damage,
		Armor,
		MissChance,
		RunningSpeed,
		WalkingSpeed,
		AttackCooldown
	}

	public class StatModifier
	{
		public readonly float Value;
		public readonly StatModType Type;
		public readonly TargetStat TargetStat;
		public readonly int Order;
		public readonly object Source;

		public StatModifier(float value, StatModType type, int order, object source, TargetStat targetStat)
		{
			Value = value;
			Type = type;
			Order = order;
			Source = source;
			TargetStat = targetStat;
		}

		public StatModifier(float value, StatModType type) : this(value, type, (int)type, null, TargetStat.None) { }

		public StatModifier(float value, StatModType type, int order) : this(value, type, order, null, TargetStat.None) { }

		public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source, TargetStat.None) { }
		public StatModifier(float value, StatModType type, TargetStat targetStat) : this(value, type, (int)type, null, targetStat) { }
	}
}
