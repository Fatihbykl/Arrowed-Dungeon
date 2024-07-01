using Gameplay.Managers;
using UnityEngine;

namespace Gameplay.StatusEffectSystem
{
    public abstract class StatusEffectBase : ScriptableObject
    {
        public float duration;
        public GameObject vfxPrefab;
        public SoundClip[] soundClips;
        
        public virtual void ApplyStatus(GameObject target) { }
        public virtual void RemoveStatus() { }
    }
}
