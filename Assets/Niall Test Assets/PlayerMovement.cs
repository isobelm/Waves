using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float waterSpeed = 5f;
    public float sandSpeed = 2f;

    private float currentSpeed;
    // private bool isInWater = false;

    void Start()
    {
        currentSpeed = sandSpeed;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(moveX, moveY, 0) * currentSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            // isInWater = true;
            currentSpeed = waterSpeed;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            // isInWater = false;
            currentSpeed = sandSpeed;
        }
    }
}
