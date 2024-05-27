using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AbilitySystem
{
    [Serializable]
    public enum SkillType
    {
        NonTargetable,
        Directional,
        Regional
    }
    public abstract class AbilityBase : ScriptableObject
    {
        public float cooldown;
        public float castTime;
        public float castRange;
        public SkillType skillType;
        public Sprite icon;

        public virtual void OnCreate(GameObject owner) { }
        public virtual bool IsReady() { return true; }
        public virtual void Activate(GameObject target) { }
        public virtual void Activate(GameObject target, Vector2? direction) { }
        public virtual void BeginCooldown() { }
    }
}
