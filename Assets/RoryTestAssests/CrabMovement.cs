using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CrabMovement : MonoBehaviour
{

    public Rigidbody2D body;

    public float START_POSITION_X;
    public float START_POSITION_Y; 

    private float xInput;
    private float yInput;

    public float ROCK_SPEED;
    public float POOL_SPEED;
    public float SEA_SPEED;

    private float playerSpeed;

    public LayerMask poolMask;
    private bool inPoolLayer;
    private BoxCollider2D poolCheck;

    public Animator crabAnimator;
    private bool isMoving;

    public TextMeshProUGUI textOutput;
    
    void Start()
    {
        ResetPosition();

        playerSpeed = ROCK_SPEED;

        isMoving = crabAnimator.GetBool("isMoving");
    }

    void Update()
    {
        CheckInput();
        // UpdatePlayerSpeed();
        HandleMovement();

        isMoving = crabAnimator.GetBool("isMoving");

        Debug();
    }

    void FixedUpdate()
    {
        // CheckLayer();
    }

    void Debug()
    {
        String debugOutput = "";

        debugOutput += "isMoving: " + isMoving + "\r\n";
        debugOutput += "playerSpeed: " + playerSpeed + "\r\n";

        textOutput.text = debugOutput;
    }

    //----------------------------------------------------------------------------

    void ResetPosition()
    {
        transform.localPosition = new Vector2(START_POSITION_X, START_POSITION_Y);
        transform.localScale = new Vector3(1, 1, 1);
    }

    void CheckInput()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
    }

      void FaceInput()
    {
        float direction = Mathf.Sign(xInput);
        transform.localScale = new Vector3(direction, 1, 1);
    }

    void UpdatePlayerSpeed()
    {
        if (inPoolLayer)
        {
            playerSpeed = POOL_SPEED;
        } else
        {
            playerSpeed = ROCK_SPEED;
        }
    }

    void HandleMovement()
    {
        float xSpeed = playerSpeed * xInput;
        float ySpeed = playerSpeed * yInput;

        if (Mathf.Abs(xInput) > 0 || Mathf.Abs(yInput) > 0)
        {
            body.linearVelocity = new Vector2(xSpeed, ySpeed);
            // transform.position += new Vector3(xInput, yInput, 0) * playerSpeed * Time.deltaTime;

            FaceInput();
            crabAnimator.SetBool("isMoving", true);
        } else
        {
            crabAnimator.SetBool("isMoving", false);
        }
    }

    void CheckLayer()
    {
        // inPoolLayer = Physics2D.OverlapAreaAll(poolCheck.bounds.min, poolCheck.bounds.max, poolMask).Length > 0;
        UnityEngine.Debug.Log("isPoolLayer: " + inPoolLayer);
    }


 //----------------------------------------------------------------------------

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            ResetPosition();
        }
        else if (other.CompareTag("TidePool"))
        {
            playerSpeed = POOL_SPEED;
            
        }
        else if (other.CompareTag("Sea"))
        {
            playerSpeed = SEA_SPEED;
        }
        else
        {
            playerSpeed = ROCK_SPEED;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TidePool"))
        {
            playerSpeed = ROCK_SPEED;
        }
        else if (other.CompareTag("Sea"))
        {
            playerSpeed = ROCK_SPEED;
        }
    }
}
