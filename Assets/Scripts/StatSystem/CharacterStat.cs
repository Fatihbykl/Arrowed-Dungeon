using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace StatSystem
{
	[Serializable]
	public class CharacterStat<T>
	{
		[SerializeField]
		protected float baseValue;

		public bool useUpperBound;
		public bool useLowerBound;
		public float upperBound;
		public float lowerBound;

		protected bool isDirty = true;
		protected float value;

		public event Action StatChanged;
		
		public virtual T Value => default;

		public virtual T BaseValue
		{
			get => default;
			set {  }
		}
		

		private readonly List<StatModifier> _statModifiers;
		public readonly ReadOnlyCollection<StatModifier> statModifiers;

		public CharacterStat()
		{
			_statModifiers = new List<StatModifier>();
			statModifiers = _statModifiers.AsReadOnly();
		}

		public CharacterStat(float baseValue) : this()
		{
			this.baseValue = baseValue;
		}

		public virtual void AddModifier(StatModifier mod)
		{
			isDirty = true;
			_statModifiers.Add(mod);
			StatChanged?.Invoke();
		}

		public virtual bool RemoveModifier(StatModifier mod)
		{
			if (_statModifiers.Remove(mod))
			{
				isDirty = true;
				StatChanged?.Invoke();
				return true;
			}
			return false;
		}

		public virtual bool RemoveAllModifiersFromSource(object source)
		{
			int numRemovals = _statModifiers.RemoveAll(mod => mod.Source == source);

			if (numRemovals > 0)
			{
				isDirty = true;
				StatChanged?.Invoke();
				return true;
			}
			return false;
		}
		
		public virtual void RemoveAllModifiers()
		{
			_statModifiers.Clear();
			isDirty = true;
			StatChanged?.Invoke();
		}

		protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
		{
			if (a.Order < b.Order)
				return -1;
			else if (a.Order > b.Order)
				return 1;
			return 0; //if (a.Order == b.Order)
		}
		
		protected virtual float CalculateFinalValue()
		{
			float finalValue = baseValue;
			float sumPercentAdd = 0;

			_statModifiers.Sort(CompareModifierOrder);

			for (int i = 0; i < _statModifiers.Count; i++)
			{
				StatModifier mod = _statModifiers[i];

				if (mod.Type == StatModType.Flat)
				{
					finalValue += mod.Value;
				}
				else if (mod.Type == StatModType.PercentAdd)
				{
					sumPercentAdd += mod.Value;

					if (i + 1 >= _statModifiers.Count || _statModifiers[i + 1].Type != StatModType.PercentAdd)
					{
						finalValue *= 1 + sumPercentAdd;
						sumPercentAdd = 0;
					}
				}
				else if (mod.Type == StatModType.PercentMult)
				{
					finalValue *= 1 + mod.Value;
				}
			}

			return finalValue;
		}
	}
}
