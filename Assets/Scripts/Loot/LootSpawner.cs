using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FSM;
using UnityEngine;

namespace Loot
{
    public class LootSpawner : MonoBehaviour
    {
        public List<GameObject> loots;
        public Transform spawnPosition;
        public float delayAfterEveryItem;
        
        private bool _hasBeenCollected;

        private void OnTriggerEnter(Collider other)
        {
            OpenChest();
        }

        public void OpenChest()
        {
            if (_hasBeenCollected) { return; }
            
            GetComponent<Animator>().SetTrigger(AnimationParameters.OpenChest);
        }

        public void Loot()
        {
            if (_hasBeenCollected) { return; }

            _hasBeenCollected = true;
            SpawnItems();
        }

        private async void SpawnItems()
        {
            for (int i = 0; i < loots.Count; i++)
            {
                var loot = Instantiate(loots[i]);
                loot.transform.position = spawnPosition.position;
                //loot.GetComponent<Rigidbody>().AddForce(transform.forward * 3, ForceMode.Impulse);
                await UniTask.WaitForSeconds(delayAfterEveryItem);
            }
        }
    }
}
