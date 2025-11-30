using System;
using UnityEngine;

public class Player2DMovement : MonoBehaviour
{

    private Rigidbody2D body;

    public float START_POSITION_X;
    public float START_POSITION_Y;

    private float xInput;
    private float yInput;

    public float ROCK_SPEED;
    public float POOL_SPEED;
    public float SEA_SPEED;

    private float currentSpeed;
    private GameStateController gameStateController;

    public Animator crabAnimator;
    public SpriteRenderer spriteRenderer;
    private GameObject sea;
    float seaStartPosY;                                                                      
    private bool isMoving;
    bool inSea = false;

    private Vector2 seaMovement = new Vector2(0f, 0f);                                                      

    void Start()
    {
        sea  = GameObject.Find("Sea");
        seaStartPosY = sea.transform.localPosition.y;

        body = GetComponent<Rigidbody2D>();
        gameStateController = FindFirstObjectByType<GameStateController>();

        ResetPosition();

        currentSpeed = POOL_SPEED;
    }

    void Update()
    {
        if (!gameStateController.GetIsGamePaused())
        {
            CheckInput();
            HandleSea();
            HandleMovement();
        }
    }

    void FixedUpdate()
    {

    }

    //----------------------------------------------------------------------------

    void ResetPosition()
    {
        transform.localPosition = new Vector2(START_POSITION_X, START_POSITION_Y);
        transform.localScale = new Vector3(1, 1, 1);
    }

    void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    void FaceInput()
    {
        float direction = Mathf.Sign(xInput);
        transform.localScale = new Vector3(direction, 1, 1);
    }

    void HandleMovement()
    {
        Vector2 movement = new Vector2(xInput, yInput) * currentSpeed;
        body.linearVelocity = movement + seaMovement;

        if (Mathf.Abs(xInput) > 0)
        {
            FaceInput();
        }

        if (Mathf.Abs(xInput) > 0 || Mathf.Abs(yInput) > 0)
        {
            crabAnimator.SetBool("isMoving", true);
            isMoving = true;
        }
        else
        {
            crabAnimator.SetBool("isMoving", false);
            isMoving = false;
        }
    }

    void HandleSea(){
        float seaYVector = sea.transform.localPosition.y;

        float seaSpeed = sea.GetComponent<Wave2DMovement>().waveSpeed;

        if(seaSpeed < 0){
            seaSpeed = seaSpeed/2;
        } else {
            seaSpeed = 2*seaSpeed/3;
        }

        if(inSea){
            seaMovement = new Vector2(0f, seaSpeed);
        } else {
            seaMovement = new Vector2(0f, 0f);
        }
    }

    //----------------------------------------------------------------------------

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            gameStateController.PlayerDied();
        }
        else if (other.CompareTag("Rock"))
        {
            currentSpeed = ROCK_SPEED;
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Crab");
        }
        else if (other.CompareTag("Sea"))
        {   
            inSea = true;
        }
        else
        {
            currentSpeed = POOL_SPEED;
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Bottom of pool");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Rock"))
        {
            currentSpeed = POOL_SPEED;
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Bottom of pool");
        }
        else if (other.CompareTag("Sea"))
        {
            inSea = false;
        }
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public Boolean GetIsMoving()
    {
        return isMoving;
    }
}
