using System;
using UnityEngine;
using UnityEngine.AI;

namespace Gameplay.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Player.Player player;
        [SerializeField] private float sphereRadius;
        
        private LayerMask playerMask;
        private NavMeshAgent agent;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            playerMask = LayerMask.GetMask("Player");
        }

        private void Update()
        {
            if (Physics.CheckSphere(transform.position, sphereRadius, playerMask, QueryTriggerInteraction.Collide))
            {
                agent.destination = player.transform.position;
                Debug.Log("test");
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, sphereRadius);
        }
    }
}