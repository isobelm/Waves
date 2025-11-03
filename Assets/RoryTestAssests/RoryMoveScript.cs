using UnityEngine;

public class RoryMoveScript : MonoBehaviour
{
    public float moveSpeed;
    public static float START_POSITION_X = 0;
    public static float START_POSITION_Y = 0;
    public OnCollisionCrab onCollisionCrab;
    bool resetCrab;

    void Start()
    {
        moveSpeed = 10;
        //transform.Translate(new Vector2(START_POSITION_X, START_POSITION_Y));
        transform.localPosition = new Vector2(0, 0);
        resetCrab = false;
    }

    void Update()
    {
        if (resetCrab) {
            transform.localPosition = new Vector2(START_POSITION_X, START_POSITION_Y);
            resetCrab = false; 
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            resetCrab = true;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            resetCrab = true;
        }
    }
}
