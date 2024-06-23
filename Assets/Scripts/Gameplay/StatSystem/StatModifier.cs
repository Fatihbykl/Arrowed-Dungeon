using System;
using UnityEngine;

namespace StatSystem
{
	public enum StatModType
	{
		Flat = 100,
		PercentAdd = 200,
		PercentMult = 300,
	}
	
	[Serializable]
	public class StatModifier
	{
		public float Value;
		public StatModType Type;
		[Tooltip("Only use with player stats! It's for matching item with stats.")]
		public StatID TargetStat;
		public int Order;
		public object Source;

		public StatModifier(float value, StatModType type, int order, object source, StatID targetStat)
		{
			Value = value;
			Type = type;
			Order = order;
			Source = source;
			TargetStat = targetStat;
		}

		public StatModifier(float value, StatModType type) : this(value, type, (int)type, null, StatID.None) { }

		public StatModifier(float value, StatModType type, int order) : this(value, type, order, null, StatID.None) { }

		public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source, StatID.None) { }
		public StatModifier(float value, StatModType type, StatID targetStat) : this(value, type, (int)type, null, targetStat) { }
	}
}
