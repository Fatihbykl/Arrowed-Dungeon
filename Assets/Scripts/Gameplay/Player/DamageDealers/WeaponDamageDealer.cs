using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageDealer : MonoBehaviour
{
    private bool canDealDamage;
    private List<GameObject> hasDealtDamage;

    [SerializeField] private float weaponLength;
    [SerializeField] private float weaponDamage;
    [SerializeField] private Vector3 raycastRotationVector;

    private void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();
    }

    private void Update()
    {
        if (canDealDamage)
        {
            RaycastHit hit;

            int layerMask = 1 << 9;
            if (Physics.Raycast(transform.position, raycastRotationVector, out hit, weaponLength, layerMask))
            {
                if (!hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    Debug.Log("Damage");
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
            }
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage.Clear();
    }
    
    public void EndDealDamage()
    {
        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + raycastRotationVector * weaponLength);
    }
}
