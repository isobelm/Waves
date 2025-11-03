using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float waterSpeed = 5f;
    public float sandSpeed = 2f;

    private float currentSpeed;

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
            currentSpeed = waterSpeed;
        }

        if (other.CompareTag("Enemy"))
        {
            Die();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            currentSpeed = sandSpeed;
        }
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
