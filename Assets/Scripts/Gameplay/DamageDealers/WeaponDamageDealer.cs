using System.Collections.Generic;
using Gameplay.Interfaces;
using Gameplay.Managers;
using UnityEngine;

namespace Gameplay.DamageDealers
{
    public class WeaponDamageDealer : MonoBehaviour
    {
        [SerializeField] private TransformTypes transformTypes;
        [SerializeField] private float weaponLength;
        [SerializeField] private LayerMask damageTo;
        [SerializeField] private SoundClip[] hitSounds;

        private List<IDamageable> _hasDealtDamage;
        private bool _canDealDamage;
        private int _weaponDamage;

        private void Start()
        {
            _canDealDamage = false;
            _hasDealtDamage = new List<IDamageable>();
        }

        private void Update()
        {
            if (_canDealDamage)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, GetTransformVector(), out hit, weaponLength, damageTo))
                {
                    IDamageable hitObject = hit.transform.gameObject.GetComponent<IDamageable>();
                    if (hitObject != null && !_hasDealtDamage.Contains(hitObject))
                    {
                        _hasDealtDamage.Add(hitObject);
                        hitObject.TakeDamage(_weaponDamage);
                        AudioManager.Instance.PlayRandomSoundFXClip(hitSounds, transform);
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
            _weaponDamage = damage;
            _canDealDamage = true;
            _hasDealtDamage.Clear();
        }
    
        public void OnEndDealDamage()
        {
            _canDealDamage = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + GetTransformVector() * weaponLength);
        }
    }
}
