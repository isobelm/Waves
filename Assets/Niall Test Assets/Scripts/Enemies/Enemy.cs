using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Abstract base class for all enemies in the game.
/// Provides common enemy functionality and interface for derived types.
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [Range(0.5f, 10f)]
    [Tooltip("Speed this enemy moves")]
    public float moveSpeed = 2f;

    [Header("Events")]
    [Tooltip("Fired when this enemy dies")]
    public UnityEvent<Enemy> OnEnemyDeath;

    protected Rigidbody2D rb;
    protected bool isAlive = true;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing Rigidbody2D component!");
        }
    }

    /// <summary>
    /// Kills this enemy and fires death event.
    /// </summary>
    public virtual void Die(string reason = "Unknown")
    {
        if (!isAlive) return;

        isAlive = false;
        Debug.Log($"{gameObject.name} died: {reason}");
        OnEnemyDeath?.Invoke(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// Abstract method for enemy-specific behavior.
    /// Derived classes must implement their unique movement/action logic.
    /// </summary>
    public abstract void UpdateBehavior();

    /// <summary>
    /// Gets the current alive state.
    /// </summary>
    public bool IsAlive() => isAlive;
}
