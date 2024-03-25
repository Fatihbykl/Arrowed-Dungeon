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

        public virtual void Activate(GameObject owner, GameObject target) { }
        public virtual void BeginCooldown(GameObject owner, GameObject target) { }
    }
}