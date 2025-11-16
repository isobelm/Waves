using UnityEngine;
using TMPro;

[RequireComponent(typeof(WaterDetector))]
[RequireComponent(typeof(PlayerHealth))]
public class SeagullTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [Range(1f, 10f)]
    [Tooltip("Seconds out of water before death")]
    public float timeBeforeDeath = 3f;

    [Range(0f, 3f)]
    [Tooltip("Seconds remaining when seagull appears")]
    public float seagullWarningTime = 1f;

    [Header("Seagull Spawning")]
    [Tooltip("Seagull prefab to spawn")]
    public GameObject seagullPrefab;

    [Tooltip("Offset from player position where seagull spawns")]
    public Vector3 seagullSpawnOffset = new Vector3(3f, 5f, 0f);

    [Range(1f, 10f)]
    [Tooltip("Speed the seagull moves toward player")]
    public float seagullApproachSpeed = 5f;

    [Header("UI")]
    [Tooltip("TextMeshPro UI element for timer display")]
    public TextMeshProUGUI timerText;
    
    private WaterDetector waterDetector;
    private PlayerHealth playerHealth;
    
    private float timeOutOfWater = 0f;
    private GameObject activeSeagull;
    
    void Start()
    {
        waterDetector = GetComponent<WaterDetector>();
        playerHealth = GetComponent<PlayerHealth>();
        
        // Subscribe to water events
        waterDetector.OnEnterWater.AddListener(OnEnteredWater);
        waterDetector.OnExitWater.AddListener(OnExitedWater);
    }
    
    void Update()
    {
        if (!waterDetector.IsInWater())
        {
            timeOutOfWater += Time.deltaTime;
            UpdateTimerDisplay();
            
            // Check for death
            if (timeOutOfWater >= timeBeforeDeath)
            {
                playerHealth.Die("Caught by seagull");
                return;
            }
            
            // Handle seagull warning
            float timeRemaining = timeBeforeDeath - timeOutOfWater;
            if (timeRemaining <= seagullWarningTime)
            {
                ShowSeagull();
            }
        }
    }
    
    void OnEnteredWater()
    {
        timeOutOfWater = 0f;
        HideSeagull();
        UpdateTimerDisplay();
    }
    
    void OnExitedWater()
    {
        // Timer starts counting
    }
    
    void ShowSeagull()
    {
        if (activeSeagull == null)
        {
            Vector3 spawnPos = transform.position + seagullSpawnOffset;
            activeSeagull = Instantiate(seagullPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            // Move seagull closer
            Vector3 targetPos = transform.position + new Vector3(0f, 2f, 0f);
            activeSeagull.transform.position = Vector3.MoveTowards(
                activeSeagull.transform.position,
                targetPos,
                seagullApproachSpeed * Time.deltaTime
            );
        }
    }
    
    void HideSeagull()
    {
        if (activeSeagull != null)
        {
            Destroy(activeSeagull);
        }
    }
    
    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            if (waterDetector.IsInWater())
            {
                timerText.text = "In water - safe!";
            }
            else
            {
                timerText.text = $"Out of water: {timeOutOfWater:F1}s / {timeBeforeDeath:F1}s";
            }
        }
    }
}
