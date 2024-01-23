using System;
using Gameplay.Interfaces;
using Gameplay.Player.DamageDealers;
using UnityEngine;

namespace Gameplay.Player
{
    public class ArrowCollider : MonoBehaviour
    {
        [SerializeField] private ArrowType arrowSO;

        private GameObject particle;

        private void OnCollisionEnter(Collision other)
        {
            IDamageable collideObject = other.gameObject.GetComponent<IDamageable>();

            if (collideObject != null)
            {
                collideObject.TakeDamage(arrowSO.baseDamage);
                if (arrowSO.hitParticlePrefab != null)
                {
                    particle = Instantiate(arrowSO.hitParticlePrefab, transform.position, transform.rotation);
                }
            }
            Destroy(this.gameObject);
            Destroy(particle, 1f);
        }
    }
}
