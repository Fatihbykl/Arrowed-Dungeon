using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace AbilitySystem
{
    public abstract class AbilityBase : ScriptableObject
    {
        public float cooldown;
        public float castTime;
        public float castRange;

        public virtual void OnCreate(GameObject owner) { }
        public virtual bool IsReady() { return true; }
        public virtual void Activate(GameObject target) { }
        public virtual void BeginCooldown() { }
    }
}
