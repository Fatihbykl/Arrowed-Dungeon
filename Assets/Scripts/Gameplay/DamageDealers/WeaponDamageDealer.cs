using System.Collections.Generic;
using Gameplay.Interfaces;
using Gameplay.Player.DamageDealers;
using UnityEngine;

namespace Gameplay.DamageDealers
{
    public class WeaponDamageDealer : MonoBehaviour
    {
        private bool canDealDamage;
        private List<IDamageable> hasDealtDamage;

        [SerializeField] private TransformTypes transformTypes;
        [SerializeField] private float weaponLength;
        [SerializeField] private LayerMask damageTo;

        private int weaponDamage;

        private void Start()
        {
            canDealDamage = false;
            hasDealtDamage = new List<IDamageable>();
        }

        private void Update()
        {
            if (canDealDamage)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, GetTransformVector(), out hit, weaponLength, damageTo))
                {
                    IDamageable hitObject = hit.transform.gameObject.GetComponent<IDamageable>();
                    if (hitObject != null && !hasDealtDamage.Contains(hitObject))
                    {
                        hasDealtDamage.Add(hitObject);
                        hitObject.TakeDamage(weaponDamage, Vector3.zero);
                    }
                }
            }
        }

        private Vector3 GetTransformVector()
        {
            Vector3 returnValue;
            switch (transformTypes)
            {
                case TransformTypes.Right:
                    returnValue = transform.right;
                    break;
                case TransformTypes.Up:
                    returnValue = transform.up;
                    break;
                case TransformTypes.NegativeUp:
                    returnValue = -transform.up;
                    break;
                case TransformTypes.Forward:
                    returnValue = transform.forward;
                    break;
                default:
                    returnValue = Vector3.zero;
                    break;
            }
            return returnValue;
        }

        public void OnStartDealDamage(int damage)
        {
            weaponDamage = damage;
            canDealDamage = true;
            hasDealtDamage.Clear();
        }
    
        public void OnEndDealDamage()
        {
            canDealDamage = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + GetTransformVector() * weaponLength);
        }
    }
}
