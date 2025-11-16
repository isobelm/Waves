using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [Tooltip("Event fired when player dies, includes death reason")]
    public UnityEvent<string> OnDeath;
    
    private bool isDead = false;
    
    public void Die(string reason)
    {
        if (isDead) return;
        
        isDead = true;
        Debug.Log($"Player died! Reason: {reason}");
        OnDeath?.Invoke(reason);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Die("Hit by enemy");
        }
    }
}