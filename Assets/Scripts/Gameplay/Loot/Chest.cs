using System;
using Animations;
using Gameplay.Managers;
using UnityEngine;

namespace Gameplay.Loot
{
    public class Chest : MonoBehaviour
    {
        public SoundClip chestOpenSound;
        
        private bool _hasBeenCollected;
        private LootSpawner _lootSpawner;

        private void Start()
        {
            _lootSpawner = GetComponent<LootSpawner>();
        }

        private void OnTriggerEnter(Collider other)
        {
            OpenChest();
        }

        public void OpenChest()
        {
            if (_hasBeenCollected) { return; }
            
            GetComponent<Animator>().SetTrigger(AnimationParameters.OpenChest);
            AudioManager.Instance.PlaySoundFXClip(chestOpenSound, transform);
        }

        public void Loot()
        {
            if (_hasBeenCollected) { return; }

            _hasBeenCollected = true;
            _lootSpawner.SpawnItems();
        }
    }
}
