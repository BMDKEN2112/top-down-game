using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private AnimationCurve accelerationCurve;

    [SerializeField] private Rigidbody2D rb2d;

    [SerializeField] private float maxSpeed = 3;
    [SerializeField] private float accelerationMaxTime = 1;
    private float buttonHeldTime;
    private bool isMoving;

    [SerializeField] private string animationMovementX = "InputX";
    [SerializeField] private string animationMovementY = "InputY";

    private Vector2 pointerInput, movementInput;

    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

    private WeaponParent weaponParent;

    private PlayerMovers playerMovers;

    public void PerformAttack()
    {
        weaponParent.Attack();
    }

    private void Awake()
    {
    	rb2d = GetComponent<Rigidbody2D>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        playerMovers = GetComponent<PlayerMovers>();
    }

    private void Update()
    {
        Vector2 input = GetInput();

        weaponParent.PointerPosition = pointerInput;
        playerMovers.MovementInput = movementInput;

        SetAnimation(input);
        SetAccelerationParameters(input);

        float speed = CalculateSpeed(input, accelerationCurve);

        SetVelocity(speed, input);
    }

    private void SetAnimation(Vector2 input)
    {
        animator.SetFloat(animationMovementX, input.x);
        animator.SetFloat(animationMovementY, input.y);
    }

    private void SetVelocity(float speed, Vector2 input)
    {
        rb2d.velocity = input.normalized * speed;
    }

    private void SetAccelerationParameters(Vector2 input)
    {
        if (input.magnitude > 0)
        {
            isMoving = true;
            buttonHeldTime += Time.deltaTime;
        }
        else
        {
            isMoving = false;
            buttonHeldTime = 0;
        }
    }

    private float CalculateSpeed(Vector2 input, AnimationCurve animationCurve)
    {
        if (isMoving)
        {
            float acceleration = accelerationCurve.Evaluate(buttonHeldTime / accelerationMaxTime);
            return maxSpeed * acceleration;
        }
        else
        {
            return 0;
        }
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
