using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounce : MonoBehaviour
{
    Vector3 lastVelocity;
    Rigidbody rb;
    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Wall" && CheckHealth())
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            rb.velocity = direction * Mathf.Max(speed, 3f);
        }
    }

    void Update()
    {
        lastVelocity = rb.velocity;
    }

    bool CheckHealth()
    {
        var obj = this.GetComponent<Ball>();
        int health = obj.health;
        bool isTouched = obj.isTouched;
        if (isTouched)
        {
            obj.health -= 1;
            obj.isTouched = false;
            health = obj.health;
            if (health <= 0)
            {
                Destroy(this.gameObject);
                return false;
            }
            else
            {
                if(obj.materials[health - 1])
                    obj.GetComponent<MeshRenderer>().material = obj.materials[health - 1];
                return true;
            }
        }
        else
            return true;
    }
}
