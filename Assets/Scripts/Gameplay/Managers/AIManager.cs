using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Managers
{
    public class AIManager : MonoBehaviour
    {
        public List<Enemy.Enemy> units = new();
        public static AIManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }

            Destroy(gameObject);
        }
    }
}
