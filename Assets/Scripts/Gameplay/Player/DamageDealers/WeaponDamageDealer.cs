using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Player;
using Gameplay.Player.DamageDealers;
using UnityEngine;

public class WeaponDamageDealer : MonoBehaviour
{
    private bool canDealDamage;
    private List<GameObject> hasDealtDamage;

    [SerializeField] private DamageDealerTypes weaponType;
    [SerializeField] private float weaponLength;
    [SerializeField] private float weaponDamage;

    private void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();

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
            int layerMask = 1 << 6; // select layer 6
            if (Physics.Raycast(transform.position, transform.right , out hit, weaponLength, layerMask))
            {
                if (!hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    Debug.Log($"Hit -> {weaponType}");
                    hasDealtDamage.Add(hit.transform.gameObject);
                }
            }
        }
    }

    private void OnStartDealDamage(DamageDealerTypes type)
    {
        if (weaponType != type) { return; }

        canDealDamage = true;
        hasDealtDamage.Clear();
    }
    
    private void OnEndDealDamage()
    {
        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * weaponLength);
    }
}
