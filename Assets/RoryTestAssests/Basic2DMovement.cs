using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic2DMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public BoxCollider2D groundCheck;
    public LayerMask groundMask;

    public float acceleration;
    [Range(0f, 1f)]
    public float groundDecay;
    public float maxXSpeed;

    public float jumpSpeed;

    public bool grounded;

    private float xInput;
    private float yInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // speed = 10;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        HandleJump();

        // Vector2 direction = new Vector2(xInput, yInput).normalized;
        // body.linearVelocity = direction * speed;        
    }

    void FixedUpdate()
    {
        CheckGround();
        HandleXMovement();
        ApplyFriction();
    }

    void CheckInput()
    {
        //Update x & y axis with key input
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

    void HandleXMovement()
    {
        if (Mathf.Abs(xInput) > 0)
        {
            //Increment velocity by acceleration, then clamp the max
            float increment = xInput * acceleration;
            float newSpeed = Mathf.Clamp(body.linearVelocity.x + increment, -maxXSpeed, maxXSpeed);

            body.linearVelocity = new Vector2(newSpeed, body.linearVelocity.y);

            FaceInput();
        }
    }

    void FaceInput()
    {
        //face horizontal direction
        float direction = Mathf.Sign(xInput);
        transform.localScale = new Vector3(direction, 1, 1);
    }

    void HandleJump()
    {
        if ((Input.GetButtonDown("Jump")) && grounded)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
        }
    }

    void CheckGround()
    {
        grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
    }

    void ApplyFriction()
    {
        if (grounded && xInput == 0 && body.linearVelocity.y <= 0)
        {
            body.linearVelocity *= groundDecay;
        }
    }
}
