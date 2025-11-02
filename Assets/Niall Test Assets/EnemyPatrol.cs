using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float waterSpeed = 2f;
    public float sandSpeed = 1f;
    public float patrolDistance = 5f;

    private float currentSpeed;
    private Vector3 startPosition;
    private bool movingRight = true;

    void Start()
    {
        currentSpeed = sandSpeed;
        startPosition = transform.position;
    }

    void Update()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * currentSpeed * Time.deltaTime;

            if (transform.position.x >= startPosition.x + patrolDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.position += Vector3.left * currentSpeed * Time.deltaTime;

            if (transform.position.x <= startPosition.x - patrolDistance)
            {
                movingRight = true;
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            currentSpeed = waterSpeed;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            currentSpeed = sandSpeed;
        }
    }
}
