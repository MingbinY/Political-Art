using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    PlayerAnimationManager animManager;

    public float moveSpeed = 5f;

    Rigidbody2D rb;
    Vector2 movementVector = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animManager = GetComponent<PlayerAnimationManager>();
    }

    private void Update()
    {
        HandleInput();
        UpdateRotation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        rb.MovePosition(rb.position + movementVector * moveSpeed * Time.fixedDeltaTime);
    }

    void HandleInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        movementVector = new Vector2(x, y);
    }

    void UpdateRotation()
    {
        animManager.currentVector = movementVector;
    }
}
