using AbilitySystem;
using UnityEngine;

namespace DataPersistance.Data.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
    public class EnemySO : ScriptableObject
    {
        [Header("Combat Settings")]
        public int enemyBaseDamage;
        public int enemyBaseHealth;
        public int enemyBaseArmor;
        public float chaseSpeed;
        public AbilityBase[] abilities;
        public bool canKnockbackable;

        [Header("Agent Settings")]
        [Tooltip("Awareness size of the enemy.")] public float sphereRadius;
        public float patrolSpeed;
        public float waypointWaitTime;
        public float stoppingDistance;
    }
}
