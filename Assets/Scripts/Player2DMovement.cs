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
    private bool isMoving;

    void Start()
    {
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
        body.linearVelocity = movement;

        // float xSpeed = currentSpeed * xInput;
        // float ySpeed = currentSpeed * yInput;

        if (Mathf.Abs(xInput) > 0)
        {
            FaceInput();
        }

        if (Mathf.Abs(xInput) > 0 || Mathf.Abs(yInput) > 0)
        {
            // body.linearVelocity = new Vector2(xSpeed, ySpeed);
            // body.linearVelocity = new Vector2(xSpeed, ySpeed);
            crabAnimator.SetBool("isMoving", true);
        }
        else
        {
            crabAnimator.SetBool("isMoving", false);
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
            currentSpeed = SEA_SPEED;
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
            currentSpeed = ROCK_SPEED;
        }
    }
}
