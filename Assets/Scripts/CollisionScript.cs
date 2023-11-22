using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    public float strength;
    Vector3 lastVelocity;
    Rigidbody rb;
    Ball arrow;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        arrow = this.GetComponent<Ball>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!arrow.isAlive)
        {
            return;
        }
        if (collision.collider.tag == "Shield")
        {
            CheckHealth(collision);
        }
        if (this.gameObject != null)
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            rb.velocity = direction * Mathf.Max(speed, 3f);
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.z, direction.x) * -Mathf.Rad2Deg + 90, 0);
        }
    }

    void Update()
    {
        lastVelocity = rb.velocity;
    }

    void CheckHealth(Collision collision)
    {
        var obj = this.GetComponent<Ball>();
        obj.health -= 1;
        int health = obj.health;
        if (health <= 0)
        {
            //Destroy(this.gameObject);
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
            direction.y = -9.81f;
            rb.velocity = direction;
            //rb.mass = 1;
            
            rb.constraints = RigidbodyConstraints.None;
            arrow.isAlive = false;
        }
        else
        {
            //change materials for arrow that has more than one health
            if (obj.materials[health - 1])
                obj.GetComponent<MeshRenderer>().material = obj.materials[health - 1];
        }
    }
}
