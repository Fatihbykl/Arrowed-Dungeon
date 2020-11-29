using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour
{
    public float strength;
    private void OnCollisionEnter(Collision collision)
    {
        strength = Random.Range(5f, 20f);
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 direction = collision.transform.position - this.transform.position;
            direction.y = 0;

            rb.AddForce(direction.normalized * strength, ForceMode.Impulse);
            collision.gameObject.GetComponent<Ball>().isTouched = true;
        }
    }
}
