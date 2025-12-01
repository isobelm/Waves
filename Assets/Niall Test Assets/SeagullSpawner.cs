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

    private float seagullY = 0;


    void Start()
    {
        playerMovement = GetComponent<Player2DMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float timeRemaining = playerMovement.timeBeforeSeagullDeath - playerMovement.GetTimeOutOfWater();
        // Debug.Log("Time Remaining: " + timeRemaining);

        // Phase 1: Spawn shadow when time remaining between 4s and 2s (second threshold)
        if (timeRemaining <= seagullStartTime && timeRemaining > shadowStartTime )
        {
            if (!shadowHasSpawned)
            {
                // Clean up any existing seagull first
                if (activeGameObject != null)
                {
                    Destroy(activeGameObject);
                    Debug.Log("Destroy Seagull 1");
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
        else if (timeRemaining <= shadowStartTime && timeRemaining > 0 )
        {
            if (activeGameObject == null || shadowHasSpawned == true)
            {
                shadowHasSpawned = false;
                // SpawnSeagull();
            }
            else
            {
                // MoveSeagullDown();
            }
        }
        else
        {
            // Clean up when not in either phase
            if (activeGameObject != null)
            {
                Destroy(activeGameObject);
                Debug.Log("Destroy Seagull 2");
                
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
        Debug.Log("Create Seagull 1");
    }

    void SpawnSeagullShadow()
    {
        // Spawn at bottom of screen below player, rotated 180 degrees
        Vector3 spawnPos = transform.position + new Vector3(0f, -10f, 0f);
        activeGameObject = Instantiate(seagullShadowPrefab, spawnPos, Quaternion.Euler(0f, 0f, 180f));
        Debug.Log("Create Seagull 2 (Shadow)");
    }

    void MoveSeagullDown()
    {
        activeGameObject.transform.position += Vector3.down * movementSpeed * Time.deltaTime;
        // Destroy shadow once it goes low enough off screen
        if (activeGameObject.transform.position.y < transform.position.y - 20f)
        {
            Destroy(activeGameObject);
            Debug.Log("Destroy Seagull 3");
            activeGameObject = null;
        }
    }

    void MoveSeagullShadowUp()
    {
        // Move shadow straight up
        activeGameObject.transform.position += Vector3.up * movementSpeed * Time.deltaTime;

        // Destroy shadow once it goes high enough off screen
        if (activeGameObject.transform.position.y > transform.position.y + 20f)
        {
            Destroy(activeGameObject);
            Debug.Log("Destroy Seagull 4");
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
