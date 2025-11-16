using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages wave spawning and coordinates wave effects across all affected entities.
/// This system is expandable for future wave mechanics integration.
/// </summary>
public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [Range(1f, 10f)]
    [Tooltip("Frequency of wave spawning in seconds")]
    public float waveFrequency = 5f;

    [Range(1f, 50f)]
    [Tooltip("Force applied by waves to entities")]
    public float waveForce = 10f;

    [Range(1f, 20f)]
    [Tooltip("Distance from top of screen that waves affect")]
    public float waveAffectRange = 5f;

    [Header("Events")]
    [Tooltip("Fired when a wave begins")]
    public UnityEvent OnWaveStart;

    [Tooltip("Fired when a wave ends")]
    public UnityEvent OnWaveEnd;

    private float waveTimer = 0f;

    void Start()
    {
        waveTimer = waveFrequency;
    }

    void Update()
    {
        waveTimer -= Time.deltaTime;

        if (waveTimer <= 0)
        {
            SpawnWave();
            waveTimer = waveFrequency;
        }
    }

    void SpawnWave()
    {
        OnWaveStart?.Invoke();
        // TODO: Implement actual wave visuals and physics
    }
}
