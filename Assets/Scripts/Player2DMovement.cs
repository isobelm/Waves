using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class Player2DMovement : MonoBehaviour
{

    public int START_POSITION_X;
    public int START_POSITION_Y;

    public float WATER_SPEED = 5f;
    public float SAND_SPEED = 2f;
    private float currentSpeed;

    private float moveX;
    private float moveY;

    private bool isInWater = false;

    private Rigidbody2D body;

    public bool IsInWater()
    {
        return isInWater;
    }

    void Start()
    {
        ResetPosition();
        currentSpeed = SAND_SPEED;
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        FaceInput();
        Vector2 movement = new Vector2(moveX, moveY) * currentSpeed;
        body.linearVelocity = movement;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Die("Red circles hurt");
        }

        if (other.CompareTag("Sand"))
        {
            isInWater = false;
            currentSpeed = SAND_SPEED;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Sand"))
        {
            isInWater = true;
            currentSpeed = WATER_SPEED;
        }
    }

    void Die(string reason)
    {
        Debug.Log($"Died because: {reason}");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void ResetPosition()
    {
        transform.localPosition = new Vector2(START_POSITION_X, START_POSITION_Y);
        transform.localScale = new Vector3(1, 1, 1);
    }

    void FaceInput()
    {
        float direction = Mathf.Sign(moveX);
        transform.localScale = new Vector3(direction, 1, 1);
    }
}
