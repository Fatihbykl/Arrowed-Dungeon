using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public ArrowType arrowType;
    public int health;
    public int coinReward;
    public float speed;
    public bool isAlive = true;

    private Rigidbody rb;
    private Vector3 lastVelocity;

    private void OnEnable()
    {
        GameplayEvents.FreezeSkillActivated += onFreezeSkillActivated;
        GameplayEvents.DestroyerSkillActivated += onDestroyerSkillActivated;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        var arrowRenderer = GetComponent<Renderer>();

        arrowRenderer.materials[0].color = arrowType.arrowBodyColor;
        arrowRenderer.materials[1].color = arrowType.arrowMetalSideColor;
        arrowRenderer.materials[2].color = arrowType.arrowTailColor;
        
        health = arrowType.baseHealth;
        coinReward = arrowType.basecoinReward;
        speed = arrowType.baseSpeed;
    }

    private void FixedUpdate()
    {
        lastVelocity = rb.velocity;
    }

    public void TakeDamage(Vector3 contactPoint, int damage = 1)
    {
        health -= damage;
        if (health <= 0)
        {
            ArrowDyingAnim(contactPoint);
            GameplayEvents.ArrowDead.Invoke(arrowType.name, arrowType.basecoinReward);
        }
    }

    public void ArrowReflect(Vector3 contactPoint)
    {
        var direction = Vector3.Reflect(lastVelocity.normalized, contactPoint);

        rb.velocity = direction * lastVelocity.magnitude;
        transform.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.z, direction.x) * -Mathf.Rad2Deg + 90, 0);
    }

    private void ArrowDyingAnim(Vector3 contactPoint)
    {
        var direction = Vector3.Reflect(lastVelocity.normalized, contactPoint);
        direction.y = -9.81f;
        rb.velocity = direction;

        rb.constraints = RigidbodyConstraints.None;
        isAlive = false;
    }

    private void onFreezeSkillActivated(float amount)
    {
        rb.velocity *= amount;
    }
    private void onDestroyerSkillActivated(int damage)
    {
        TakeDamage(new Vector3(), damage);
    }
}
