using System;
using Gameplay.Player.DamageDealers;
using UnityEngine;

namespace Gameplay.Player
{
    public class ArrowCollider : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            IDamageable collideObject = other.gameObject.GetComponent<IDamageable>();
            
            if (collideObject != null)
            {
                collideObject.TakeDamage(20);
            }
            Destroy(this.gameObject);
        }
    }
}
