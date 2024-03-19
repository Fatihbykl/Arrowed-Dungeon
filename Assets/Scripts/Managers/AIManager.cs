using System;
using System.Collections.Generic;
using Gameplay.Enemy;
using UnityEngine;

namespace Managers
{
    public class AIManager : MonoBehaviour
    {
        private static AIManager _instance;
        public static AIManager Instance
        {
            get
            {
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        public Transform Target;
        public float RadiusAroundTarget = 0.5f;
        public List<Enemy> Units = new List<Enemy>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                return;
            }

            Destroy(gameObject);
        }

        private void Update()
        {
            if (Units.Count == 0) { return; }
            
            MakeAgentsCircleTarget();
        }

        public void RemoveUnit(Enemy unit)
        {
            Units.Remove(unit);
        }

        private void MakeAgentsCircleTarget()
        {
            for (int i = 0; i < Units.Count; i++)
            {
                Units[i].agentController.agent.SetDestination(new Vector3(
                    Target.position.x + RadiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / Units.Count),
                    Target.position.y,
                    Target.position.z + RadiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / Units.Count)
                ));
            }
        }
    }
}
