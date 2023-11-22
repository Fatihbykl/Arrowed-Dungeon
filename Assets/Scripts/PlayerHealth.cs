﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health = 1;

    private Animator animator;
    private CharacterMovement characterMovement;

    private void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Ball" && collision.collider.GetComponent<Ball>().isAlive)
        {
            health -= 1;
            if (health == 0)
            {
                StartCoroutine(Die());
            }
                
        }
    }

    private IEnumerator Die()
    {
        characterMovement.isActive = false;
        // Play the animation for getting suck in
        animator.SetTrigger("death");

        yield return new WaitForSeconds(3); // oyun bitti
        Time.timeScale = 0;

    }
}
