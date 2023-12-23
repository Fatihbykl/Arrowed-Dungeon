using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    [SerializeField] private int health = 1;
    // add scene loaded event and publish initial health

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
            animator.SetTrigger("isDead");

            yield return new WaitForSeconds(2);
            characterCollider.enabled = false;
            Time.timeScale = 0;
            GameplayEvents.LevelFailed.Invoke();
        }
    }

    public void LoadData(GameData data)
    {
        health = data.health;
    }

    public void SaveData(GameData data) { return; }
}
