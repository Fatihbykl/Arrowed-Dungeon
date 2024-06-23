using UnityEngine;

namespace StatusEffectSystem
{
    public abstract class StatusEffectBase : ScriptableObject
    {
        public float duration;
        public GameObject vfxPrefab;
        
        public virtual void ApplyStatus(GameObject target) { }
        public virtual void RemoveStatus() { }
    }
}
