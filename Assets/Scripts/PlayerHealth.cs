using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health = 1;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Ball")
        {
            health -= 1;
            if (health <= 0)
            {
                StartCoroutine(Die());
            }
                
        }
    }

    private IEnumerator Die()
    {
        // Play the animation for getting suck in
        animator.SetTrigger("death");

        yield return new WaitForSeconds(3); // oyun bitti
        Time.timeScale = 0;

    }
}
