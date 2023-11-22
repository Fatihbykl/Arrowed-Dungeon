using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 6;
    public float gravity = -9.81f;
    public Vector3 direction;
    public float turnSmoothTime = 0.1f;
    public bool isActive = true;

    private Vector3 velocity;
    private Animator animator;
    private float turnSmoothVelocity;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isActive) { return; }
        if (controller.isGrounded && velocity.y < 0){ velocity.y = -2f; }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;
        animator.SetFloat("directionMagnitude", direction.magnitude);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(direction.normalized * speed * Time.deltaTime);
        }
    }
}

