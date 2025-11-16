using UnityEngine;

/// <summary>
/// Manages seagull spawning and movement.
/// Responds to danger events to show/hide seagull.
/// </summary>
public class SeagullSpawner : MonoBehaviour
{
    [Header("Seagull Settings")]
    [Tooltip("Seagull prefab to spawn")]
    public GameObject seagullPrefab;

    [Tooltip("Offset from player position where seagull spawns")]
    public Vector3 spawnOffset = new Vector3(3f, 5f, 0f);

    [Range(1f, 10f)]
    [Tooltip("Speed the seagull moves toward player")]
    public float approachSpeed = 5f;

    [Tooltip("Target position offset (usually above player)")]
    public Vector3 targetOffset = new Vector3(0f, 2f, 0f);

    private GameObject activeSeagull;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = transform;
    }

    void Update()
    {
        if (activeSeagull != null)
        {
            MoveSeagullTowardPlayer();
        }
    }

    /// <summary>
    /// Shows the seagull and starts moving it toward player.
    /// </summary>
    public void ShowSeagull()
    {
        if (activeSeagull == null)
        {
            Vector3 spawnPos = playerTransform.position + spawnOffset;
            activeSeagull = Instantiate(seagullPrefab, spawnPos, Quaternion.identity);
        }
    }

    /// <summary>
    /// Hides and destroys the active seagull.
    /// </summary>
    public void HideSeagull()
    {
        if (activeSeagull != null)
        {
            Destroy(activeSeagull);
            activeSeagull = null;
        }
    }

    void MoveSeagullTowardPlayer()
    {
        Vector3 targetPos = playerTransform.position + targetOffset;
        activeSeagull.transform.position = Vector3.MoveTowards(
            activeSeagull.transform.position,
            targetPos,
            approachSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// Checks if seagull is currently active.
    /// </summary>
    public bool IsSeagullActive() => activeSeagull != null;
}
