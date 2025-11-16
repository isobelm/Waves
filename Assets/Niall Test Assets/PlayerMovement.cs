using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float waterSpeed = 5f;
    public float sandSpeed = 2f;
    public float timeBeforeSeagullDeath = 3f;

    public TextMeshProUGUI timerText;

    private float currentSpeed;
    private bool isInWater = false;
    private Rigidbody2D rb;

    private float timeOutOfWater = 0f;

    public float GetTimeOutOfWater()
    {
        return timeOutOfWater;
    }

    public bool IsInWater()
    {
        return isInWater;
    }

    void Start()
    {
        currentSpeed = sandSpeed;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(moveX, moveY) * currentSpeed;
        rb.linearVelocity = movement;

        if (!isInWater)
        {
            timeOutOfWater += Time.deltaTime;

            if (timerText != null)
            {
                timerText.text = $"Out of water: {timeOutOfWater:F1}s";
            }

            if (timeOutOfWater >= timeBeforeSeagullDeath)
            {
                Die("Seagull did what seagull do");
            }
        }
        else {
            timeOutOfWater = 0f;

            if (timerText != null)
            {
                timerText.text = "In water";
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            currentSpeed = waterSpeed;
        }

        if (other.CompareTag("Enemy"))
        {
            Die("Red circles hurt");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            currentSpeed = sandSpeed;
        }
    }

    void Die(string reason)
    {
        Debug.Log($"Died because: {reason}");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
