using System;
using NaughtyAttributes;
using UnityEngine;

namespace Gameplay.AbilitySystem
{
    [Serializable]
    public enum SkillType
    {
        NonTargetable,
        Directional,
        Regional
    }
    public abstract class AbilityBase : ScriptableObject, IComparable<AbilityBase>
    {
        [ShowAssetPreview]
        public Sprite icon;
        public string title;
        [TextArea]
        public string description;
        public float cooldown;
        public float castTime;
        public float castRange;
        public SkillType skillType;

        public virtual void OnCreate(GameObject owner) { }
        public virtual bool IsReady() { return true; }
        public virtual void Activate(GameObject target) { }
        public virtual void Activate(GameObject target, Vector3 direction) { }
        public virtual void BeginCooldown() { }
        public int CompareTo(AbilityBase other)
        {
            return other == null ? 1 : 0;
        }
    }
}
