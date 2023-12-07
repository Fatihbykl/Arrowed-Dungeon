using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health = 1;

    private Animator animator;
    private CharacterMovement characterMovement;
    private Collider characterCollider;
    private bool isDieAnimActive = false;

    private void Start()
    {
        characterMovement = GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
        characterCollider = GetComponent<Collider>();
    }

    public void TakeDamage(int damage = 1)
    {
        health -= damage;
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
        GameplayEvents.PlayerGetDamaged.Invoke(health);
    }

    private IEnumerator Die()
    {
        if (!isDieAnimActive)
        {
            isDieAnimActive = true;
            characterMovement.isActive = false;
            // Play the animation for getting suck in
            animator.SetTrigger("death");

            yield return new WaitForSeconds(2); // oyun bitti
            characterCollider.enabled = false;
            Time.timeScale = 0;
            GameplayEvents.LevelFailed.Invoke();
        }
    }
}
