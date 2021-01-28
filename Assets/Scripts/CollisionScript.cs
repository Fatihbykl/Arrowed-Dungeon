using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    public float strength;
    Vector3 lastVelocity;
    Rigidbody rb;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Shield")
        {
            CheckHealth();
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

    bool CheckHealth()
    {
        var obj = this.GetComponent<Ball>();
        obj.health -= 1;
        int health = obj.health;
        if (health <= 0)
        {
            Destroy(this.gameObject);
            return false;
        }
        else
        {
            //change materials for arrow that has more than one health
            if (obj.materials[health - 1])
                obj.GetComponent<MeshRenderer>().material = obj.materials[health - 1];
            return true;
        }
    }
}
