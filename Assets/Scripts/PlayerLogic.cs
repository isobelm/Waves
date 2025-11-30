using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class PlayerLogic : MonoBehaviour
{
    private Rigidbody2D body;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Debug.Log("body.linearVelocityY = " + body.linearVelocityY);
    }

    void HandlePlayerInsideWave()
    {
        // body.linearVelocityY *= -10f;
        body.linearVelocity = new Vector2(0f, 10f);
        // Debug.Log("body.linearVelocityY = " + body.linearVelocityY);
    }

    //---------------------------------------------------------------------------------------------------------------------

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("other.tag = " + other.tag);
        if (other.CompareTag("Sea"))
        {
            // Debug.Log("handlePlayerInsideWave()");
            HandlePlayerInsideWave();
        }
    }
}