using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(WaterDetector))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Range(1f, 10f)]
    [Tooltip("Speed when moving in water")]
    public float waterSpeed = 5f;

    [Range(0.5f, 5f)]
    [Tooltip("Speed when moving on sand")]
    public float sandSpeed = 2f;

    private Rigidbody2D rb;
    private WaterDetector waterDetector;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        waterDetector = GetComponent<WaterDetector>();
    }

    void Update()
    {
        HandleMovementInput();
    }
    
    void HandleMovementInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        
        float currentSpeed = waterDetector.IsInWater() ? waterSpeed : sandSpeed;
        Vector2 movement = new Vector2(moveX, moveY) * currentSpeed;
        rb.linearVelocity = movement;
    }
}