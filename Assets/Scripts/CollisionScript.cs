using System.Collections;
using System.Collections.Generic;
using Gameplay.Player;
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
            Player player = collision.collider.gameObject.GetComponentInParent<Player>();
            if (arrow.arrowType.type == ArrowTypeName.Killer)
            {
                player.TakeDamage(50, Vector3.zero);
            }
            else
            {
                player.TakeDamage(20, Vector3.zero);
                arrow.ArrowReflect(contactPoint);
            }
        }
        else if (collision.collider.tag == "Shield")
        {
            if (arrow.arrowType.type == ArrowTypeName.Killer)
            {
                Player player = collision.collider.gameObject.GetComponentInParent<Player>();
                player.TakeDamage(50, Vector3.zero);
            }
            else if (arrow.arrowType.type == ArrowTypeName.ShieldBreaker) 
            {
                // break the shield
            }
            else
            {
                arrow.TakeDamage(contactPoint);
                arrow.ArrowReflect(contactPoint);
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
