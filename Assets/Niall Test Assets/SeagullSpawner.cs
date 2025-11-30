using UnityEngine;

[RequireComponent(typeof(Player2DMovement))]
public class SeagullSpawner : MonoBehaviour
{
    public GameObject seagullPrefab;
    public GameObject seagullShadowPrefab;
    public float shadowStartTime = 2f;
    public float seagullStartTime = 4f;
    public float movementSpeed = 10f;

    private GameObject activeGameObject;
    private Player2DMovement playerMovement;
    private bool shadowHasSpawned = false;
    public Animator animator;


    void Start()
    {
        playerMovement = GetComponent<Player2DMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float timeRemaining = playerMovement.timeBeforeSeagullDeath - playerMovement.GetTimeOutOfWater();

        // Phase 1: Spawn shadow when time remaining between 4s and 2s (second threshold)
        if (timeRemaining <= seagullStartTime && timeRemaining > shadowStartTime && !playerMovement.IsInWater())
        {
            if (!shadowHasSpawned)
            {
                // Clean up any existing seagull first
                if (activeGameObject != null)
                {
                    Destroy(activeGameObject);
                }
                SpawnSeagullShadow();
                shadowHasSpawned = true;
            }
            else if (activeGameObject != null)
            {
                MoveSeagullShadowUp();
            }
        }
        // Phase 2: Spawn real seagull when time remaining <= shadowStartTime (2s, first threshold)
        else if (timeRemaining <= shadowStartTime && timeRemaining > 0 && !playerMovement.IsInWater())
        {
            if (activeGameObject == null || shadowHasSpawned == true)
            {
                shadowHasSpawned = false;
                SpawnSeagull();
            }
            else
            {
                MoveSeagullCloser();
            }
        }
        else
        {
            // Clean up when not in either phase
            if (activeGameObject != null)
            {
                Destroy(activeGameObject);
                activeGameObject = null;
            }
            shadowHasSpawned = false;
        }
    }

    void SpawnSeagull()
    {
        // Spawn at top of screen above player, no rotation
        Vector3 spawnPos = transform.position + new Vector3(0f, 10f, 0f);
        activeGameObject = Instantiate(seagullPrefab, spawnPos, Quaternion.Euler(0f, 0f, 0f));
    }

    void SpawnSeagullShadow()
    {
        // Spawn at bottom of screen below player, rotated 180 degrees
        Vector3 spawnPos = transform.position + new Vector3(0f, -10f, 0f);
        activeGameObject = Instantiate(seagullShadowPrefab, spawnPos, Quaternion.Euler(0f, 0f, 180f));
    }

    void MoveSeagullCloser()
    {
        // Move seagull down from top to player position
        Vector3 targetPos = transform.position;
        activeGameObject.transform.position = Vector3.MoveTowards(
            activeGameObject.transform.position,
            targetPos,
            movementSpeed * Time.deltaTime
        );
    }

    void MoveSeagullShadowUp()
    {
        // Move shadow straight up
        activeGameObject.transform.position += Vector3.up * movementSpeed * Time.deltaTime;

        // Destroy shadow once it goes high enough off screen
        if (activeGameObject.transform.position.y > transform.position.y + 20f)
        {
            Destroy(activeGameObject);
            activeGameObject = null;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   
            // playerMovement.spriteRenderer.enabled = false;
            animator.SetBool("bite", true);
        }
    }
}
