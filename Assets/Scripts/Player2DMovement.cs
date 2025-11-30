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
    private bool inSea = false;

    private Vector2 seaMovement = new Vector2(0f, 0f);     

    public float timeBeforeSeagullDeath = 5f;
    private float timeOutOfWater = 0f;
    private bool isOffRock = true;
    
    public float GetTimeOutOfWater()
    {
        return timeOutOfWater;
    }

    public bool IsInWater()
    {
        return isOffRock || inSea;
    }

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
            HandleSeagull();
            HandleMovement();
        }

        Debug.Log("timeOutOfWater: " + timeOutOfWater);
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
        }
        else
        {
            crabAnimator.SetBool("isMoving", false);
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

    void HandleSeagull(){
        if (!IsInWater())
        {
            timeOutOfWater += Time.deltaTime;

            if (timeOutOfWater >= timeBeforeSeagullDeath)
            {
                gameStateController.PlayerDied();
            }
        }
        else {
            timeOutOfWater = 0f;
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
            isOffRock = false;
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
            isOffRock = true;
            currentSpeed = POOL_SPEED;
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Bottom of pool");
        }
        else if (other.CompareTag("Sea"))
        {
            inSea = false;
        }
    }
}
