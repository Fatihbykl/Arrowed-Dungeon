using System;
using System.Collections.Generic;
using System.Linq;
using Animations;
using Cysharp.Threading.Tasks;
using FSM;
using NaughtyAttributes;
using UnityEngine;
using Utils;

namespace Gameplay.Loot
{
    [Serializable]
    public struct ChestLootWithWeights
    {
        public GameObject prefab;
        public int weight;
        public int count;
    }

    [Serializable]
    public struct LootIterations
    {
        public ChestLootWithWeights[] lootsWithWeight;
    }
    
    public class LootSpawner : MonoBehaviour
    {
        [InfoBox("Each element has their own weighted list. It means if you create one element and fill " +
                 "'Loots With Weight' with 3 items with weights. It will choose one of 3 items depending on their weights." +
                 "If you want to drop 2 items, create 2 different elements.")]
        public List<LootIterations> loots;
        public Transform spawnPosition;
        public float delayAfterEveryItem;
        
        private WeightedList<ChestLootWithWeights>[] _weightedLists;

        private void Awake()
        {
            _weightedLists = new WeightedList<ChestLootWithWeights>[loots.Count];

            for (int iteration = 0; iteration < loots.Count; iteration++)
            {
                for (int lootIndex = 0; lootIndex < loots[iteration].lootsWithWeight.Length; lootIndex++)
                {
                    _weightedLists[iteration] = new WeightedList<ChestLootWithWeights>();
                    
                    var lootObject = loots[iteration].lootsWithWeight[lootIndex];
                    _weightedLists[iteration].Add(lootObject, lootObject.weight);
                }
            }
        }

        public async void SpawnItems()
        {
            for (int i = 0; i < _weightedLists.Length; i++)
            {
                var random = _weightedLists[i].Next();

                for (int j = 0; j < random.count; j++)
                {
                    var loot = Instantiate(random.prefab);
                    loot.transform.position = spawnPosition.position;
                }
                //loot.GetComponent<Rigidbody>().AddForce(transform.forward * 3, ForceMode.Impulse);
                await UniTask.WaitForSeconds(delayAfterEveryItem);
            }
        }
    }
}
