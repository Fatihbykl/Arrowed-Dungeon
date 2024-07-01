using System;
using DataPersistance.Data.ScriptableObjects;
using Gameplay.Interfaces;
using Gameplay.Managers;
using UnityEngine;

namespace Gameplay.Player
{
    public class ArrowCollider : MonoBehaviour
    {
        [SerializeField] private ArrowType arrowSO;
        [SerializeField] private GameObject trail;
        private GameObject particle;

        private void OnTriggerEnter(Collider other)
        {
            IDamageable collideObject = other.gameObject.GetComponent<IDamageable>();

            if (collideObject != null)
            {
                Vector3 direction = (other.transform.position - transform.position).normalized * arrowSO.knockbackForce;
                direction.y = 0;
                collideObject.TakeDamage(arrowSO.baseDamage);
             
                if (arrowSO.hitParticlePrefab != null)
                {
                    particle = Instantiate(arrowSO.hitParticlePrefab, transform.position, transform.rotation);
                }
            }

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, 2f);
        }
    }
}
