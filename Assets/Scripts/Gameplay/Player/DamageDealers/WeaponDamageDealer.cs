using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Player.DamageDealers
{
    public class WeaponDamageDealer : MonoBehaviour
    {
        private bool canDealDamage;
        private List<IDamageable> hasDealtDamage;

        [SerializeField] private TransformTypes transformTypes;
        [SerializeField] private float weaponLength;
        [SerializeField] private int weaponDamage;
        [SerializeField] private LayerMask damageTo;
        [SerializeField] private GameObject rootObject;

        private void Start()
        {
            canDealDamage = false;
            hasDealtDamage = new List<IDamageable>();

            GameplayEvents.StartDealDamage += OnStartDealDamage;
            GameplayEvents.EndDealDamage += OnEndDealDamage;
        }

        private void OnDisable()
        {
            GameplayEvents.StartDealDamage -= OnStartDealDamage;
            GameplayEvents.EndDealDamage -= OnEndDealDamage;
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
                        hitObject.TakeDamage(weaponDamage);
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

        private void OnStartDealDamage(GameObject sender)
        {
            if (rootObject != sender) { return; }
            canDealDamage = true;
            hasDealtDamage.Clear();
        }
    
        private void OnEndDealDamage(GameObject sender)
        {
            if (rootObject != sender) { return; }
            canDealDamage = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + GetTransformVector() * weaponLength);
        }
    }
}
