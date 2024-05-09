using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public ArrowType arrowType;
    public int health;
    public int coinReward;
    public float speed;
    public bool isAlive = true;

    private Rigidbody _rb;
    private Vector3 _lastVelocity;

    private void OnEnable()
    {
        GameplayEvents.FreezeSkillActivated += onFreezeSkillActivated;
        GameplayEvents.DestroyerSkillActivated += onDestroyerSkillActivated;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

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
        _lastVelocity = _rb.velocity;
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
        var direction = Vector3.Reflect(_lastVelocity.normalized, contactPoint);

        _rb.velocity = direction * _lastVelocity.magnitude;
        transform.rotation = Quaternion.Euler(0, Mathf.Atan2(direction.z, direction.x) * -Mathf.Rad2Deg + 90, 0);
    }

    private void ArrowDyingAnim(Vector3 contactPoint)
    {
        var direction = Vector3.Reflect(_lastVelocity.normalized, contactPoint);
        direction.y = -9.81f;
        _rb.velocity = direction;

        _rb.constraints = RigidbodyConstraints.None;
        isAlive = false;
    }

    private void onFreezeSkillActivated(float amount)
    {
        _rb.velocity *= amount;
    }
    private void onDestroyerSkillActivated(int damage)
    {
        TakeDamage(new Vector3(), damage);
    }
}
