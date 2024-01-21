using System;
using Gameplay.Interfaces;
using Gameplay.Player.DamageDealers;
using UnityEngine;

namespace Gameplay.Player
{
    public class ArrowCollider : MonoBehaviour
    {
        [SerializeField] private ArrowType arrowSO;

        private void Start()
        {
            
        }

        private void OnCollisionEnter(Collision other)
        {
            IDamageable collideObject = other.gameObject.GetComponent<IDamageable>();

            if (collideObject != null)
            {
                collideObject.TakeDamage(arrowSO.baseDamage);
            }
            Destroy(this.gameObject);
        }
    }
}
