using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    private Arrow arrow;

    private void Start()
    {
        arrow = this.GetComponent<Arrow>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!arrow.isAlive || this.gameObject == null) { return; }

        var contactPoint = collision.GetContact(0).normal;
        if(collision.collider.tag == "Player")
        {
            PlayerHealth player = collision.collider.gameObject.GetComponentInParent<PlayerHealth>();
            if (arrow.arrowType.type == ArrowTypeName.Killer)
            {
                player.TakeDamage(50);
            }
            else
            {
                player.TakeDamage();
            }
        }
        else if (collision.collider.tag == "Shield")
        {
            if (arrow.arrowType.type == ArrowTypeName.Killer)
            {
                PlayerHealth player = collision.collider.gameObject.GetComponentInParent<PlayerHealth>();
                player.TakeDamage(50);
            }
            else if (arrow.arrowType.type == ArrowTypeName.ShieldBreaker) 
            {
                // break the shield
            }
            else
            {
                arrow.TakeDamage(contactPoint);
            }
        }
        else if (collision.collider.tag == "Wall")
        {
            if (arrow.arrowType.type == ArrowTypeName.Killer)
            {
                arrow.TakeDamage(contactPoint, 50);
            }
            else
            {
                arrow.ArrowReflect(contactPoint);
            }
        }
    }
}
