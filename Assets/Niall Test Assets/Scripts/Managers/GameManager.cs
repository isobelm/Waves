using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Global game manager handling scene transitions and game-wide state.
/// Implements singleton pattern.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Find player and subscribe to death event
        PlayerHealth playerHealth = FindAnyObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnDeath.AddListener(OnPlayerDeath);
        }
    }
    
    void OnPlayerDeath(string reason)
    {
        Debug.Log($"GameManager handling death: {reason}");
        Invoke(nameof(RestartLevel), 1f); // Wait 1 second before restarting
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
