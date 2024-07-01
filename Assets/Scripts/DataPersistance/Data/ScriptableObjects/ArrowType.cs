using UnityEngine;

namespace DataPersistance.Data.ScriptableObjects
{
    public enum ArrowTypeName
    {
        Base,
        Advanced,
        Superior,
        ShieldBreaker,
        Killer
    }

    [CreateAssetMenu(fileName = "New Arrow Type", menuName = "Arrow Type")]
    public class ArrowType : ScriptableObject
    {
        public ArrowTypeName type;

        [Header("Colors")]
        public Color arrowBodyColor = Color.white;
        public Color arrowMetalSideColor = Color.white;
        public Color arrowTailColor = Color.white;

        [Header("Settings")]
        public int baseHealth = 1;
        public int baseDamage = 1;
        public int basecoinReward = 1;
        public float baseSpeed = 250;
        public float spawnSeconds = 2;
        public float knockbackForce = 1;

        public GameObject hitParticlePrefab;
    }
}