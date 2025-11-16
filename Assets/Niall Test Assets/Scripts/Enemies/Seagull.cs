using UnityEngine;

/// <summary>
/// Seagull enemy that dives toward the player.
/// Used as the threat that kills player when out of water too long.
/// </summary>
public class Seagull : Enemy
{
    [Header("Seagull Behavior")]
    [Range(1f, 10f)]
    [Tooltip("Speed seagull moves toward target")]
    public float diveSpeed = 5f;

    [Tooltip("Target position to move toward")]
    public Vector3 targetPosition = Vector3.zero;

    private bool hasTarget = false;

    protected override void Start()
    {
        base.Start();
        // Seagull-specific initialization if needed
    }

    void Update()
    {
        if (isAlive)
        {
            UpdateBehavior();
        }
    }

    /// <summary>
    /// Updates seagull behavior - moves toward target if set.
    /// </summary>
    public override void UpdateBehavior()
    {
        if (hasTarget)
        {
            MoveTowardTarget();
        }
    }

    void MoveTowardTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            diveSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// Sets the target position for the seagull to move toward.
    /// </summary>
    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        hasTarget = true;
    }

    /// <summary>
    /// Clears the target.
    /// </summary>
    public void ClearTarget()
    {
        hasTarget = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Die("Caught by seagull");
            }
        }
    }
}
