using Gameplay.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay.Loot
{
    public class LootDrop : MonoBehaviour
    {
        public SoundClip lootCollectSound;
        
        private Vector3 _velocity = Vector3.up;
        private Rigidbody _rb;

        public void ThrowLoot()
        {
            _velocity *= Random.Range(3f, 5f);
            _velocity += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(0f, -3f));
            _rb = GetComponent<Rigidbody>();
            _rb.AddForce(_velocity, ForceMode.Impulse);
        }
    }
}
