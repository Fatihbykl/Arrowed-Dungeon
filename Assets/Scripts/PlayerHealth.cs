using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int health = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Ball")
        {
            health -= 1;
            if (health <= 0)
                Time.timeScale = 0; // oyun bitti
        }
    }
}
