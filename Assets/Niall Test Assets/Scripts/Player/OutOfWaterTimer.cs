using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Tracks how long the player has been out of water.
/// Fires events to notify other systems when danger/timeout occurs.
/// </summary>
[RequireComponent(typeof(WaterDetector))]
public class OutOfWaterTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    [Range(1f, 10f)]
    [Tooltip("Seconds out of water before timeout")]
    public float timeBeforeDeath = 3f;

    [Range(0f, 3f)]
    [Tooltip("Seconds remaining when danger state begins")]
    public float dangerThreshold = 1f;

    [Header("Events")]
    [Tooltip("Fired every frame with time remaining (0 to timeBeforeDeath)")]
    public UnityEvent<float> OnTimerTick;

    [Tooltip("Fired when entering danger zone (time < dangerThreshold)")]
    public UnityEvent OnDangerStart;

    [Tooltip("Fired when exiting danger zone")]
    public UnityEvent OnDangerEnd;

    [Tooltip("Fired when timer runs out")]
    public UnityEvent OnTimeout;

    private WaterDetector waterDetector;
    private float timeOutOfWater = 0f;
    private bool isDanger = false;

    void Start()
    {
        waterDetector = GetComponent<WaterDetector>();
        waterDetector.OnEnterWater.AddListener(ResetTimer);
        waterDetector.OnExitWater.AddListener(OnLeftWater);
    }

    void Update()
    {
        if (!waterDetector.IsInWater())
        {
            timeOutOfWater += Time.deltaTime;

            // Fire tick event
            OnTimerTick?.Invoke(timeOutOfWater);

            // Check for danger state
            float timeRemaining = timeBeforeDeath - timeOutOfWater;
            bool shouldBeDanger = timeRemaining <= dangerThreshold && timeRemaining > 0;

            if (shouldBeDanger && !isDanger)
            {
                isDanger = true;
                OnDangerStart?.Invoke();
            }
            else if (!shouldBeDanger && isDanger)
            {
                isDanger = false;
                OnDangerEnd?.Invoke();
            }

            // Check for timeout
            if (timeOutOfWater >= timeBeforeDeath)
            {
                OnTimeout?.Invoke();
                timeOutOfWater = timeBeforeDeath; // Clamp to max
            }
        }
    }

    void ResetTimer()
    {
        timeOutOfWater = 0f;
        if (isDanger)
        {
            isDanger = false;
            OnDangerEnd?.Invoke();
        }
    }

    void OnLeftWater()
    {
        // Timer starts counting in Update
    }

    /// <summary>
    /// Gets the current time out of water.
    /// </summary>
    public float GetTimeOutOfWater() => timeOutOfWater;

    /// <summary>
    /// Gets the remaining time before death.
    /// </summary>
    public float GetTimeRemaining() => Mathf.Max(0, timeBeforeDeath - timeOutOfWater);

    /// <summary>
    /// Gets the current danger state.
    /// </summary>
    public bool IsDanger() => isDanger;
}
