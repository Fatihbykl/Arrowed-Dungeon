using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private GameObject joystickObject;

    private VisualElement m_JoystickBack;
    private VisualElement m_JoystickHandle;
    private Vector2 m_JoystickPointerDownPosition;
    private Vector2 m_JoystickDelta; // Between -1 and 1

    public CharacterController controller;
    public float speed = 3;
    public float gravity = -9.81f;
    public Vector3 direction;
    public float turnSmoothTime = 0.1f;
    public bool isActive = true;

    private Vector3 velocity;
    private Animator animator;
    private float turnSmoothVelocity;


    private void Start()
    {
        if (joystickObject == null)
        {
            return;
        }

        speed = GameManager.instance.playerSpeed;
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if (joystickObject == null)
        {
            return;
        }

        var root = joystickObject.GetComponent<UIDocument>().rootVisualElement;
        m_JoystickBack = root.Q("JoystickBack");
        m_JoystickHandle = root.Q("JoystickHandle");
        m_JoystickHandle.RegisterCallback<PointerDownEvent>(OnPointerDown);
        m_JoystickHandle.RegisterCallback<PointerUpEvent>(OnPointerUp);
        m_JoystickHandle.RegisterCallback<PointerMoveEvent>(OnPointerMove);
    }

    void Update()
    {
        if (!isActive || joystickObject == null)
        {
            return;
        }

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //walk
        float horizontal = m_JoystickDelta.x;
        float vertical = m_JoystickDelta.y;

        direction = new Vector3(horizontal, 0f, -vertical).normalized;
        animator.SetFloat("directionMagnitude", direction.magnitude);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(direction.normalized * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable_Key"))
        {
            GameManager.instance.CollectedKey();
            other.GetComponent<BoxCollider>().enabled = false;
            GameplayEvents.KeyCollected.Invoke(GameManager.instance.collectedKeyCount,
                GameManager.instance.totalKeyCount, other.gameObject, this.gameObject);
        }
    }

    void OnPointerDown(PointerDownEvent e)
    {
        m_JoystickHandle.CapturePointer(e.pointerId);
        m_JoystickPointerDownPosition = e.position;
    }

    void OnPointerUp(PointerUpEvent e)
    {
        m_JoystickHandle.ReleasePointer(e.pointerId);
        m_JoystickHandle.transform.position = Vector3.zero;
        m_JoystickDelta = Vector2.zero;
    }

    void OnPointerMove(PointerMoveEvent e)
    {
        if (!m_JoystickHandle.HasPointerCapture(e.pointerId))
            return;
        var pointerCurrentPosition = (Vector2)e.position;
        var pointerMaxDelta = (m_JoystickBack.worldBound.size - m_JoystickHandle.worldBound.size) / 2;
        var pointerDelta = Clamp(pointerCurrentPosition - m_JoystickPointerDownPosition, -pointerMaxDelta,
            pointerMaxDelta);
        m_JoystickHandle.transform.position = pointerDelta;
        m_JoystickDelta = pointerDelta / pointerMaxDelta;
    }

    static Vector2 Clamp(Vector2 v, Vector2 min, Vector2 max) =>
        new Vector2(Mathf.Clamp(v.x, min.x, max.x), Mathf.Clamp(v.y, min.y, max.y));
}