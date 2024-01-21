using UnityEngine;

namespace DataPersistance.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
    public class EnemySO : ScriptableObject
    {
        [Header("Combat Settings")]
        public int enemyBaseDamage;
        public int enemyBaseHealth;
        public float attackDelay;
        [Header("Agent Settings")]
        [Tooltip("Awareness size of the enemy.")] public float sphereRadius;
        public float patrolSpeed;
        public float chaseSpeed;
        public float waypointWaitTime;
        public float stoppingDistance;
        public float radius;
        public float height;
    }
}
