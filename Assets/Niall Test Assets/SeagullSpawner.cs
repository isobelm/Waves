using UnityEngine;

public class SeagullSpawner : MonoBehaviour
{
    public GameObject seagullPrefab;
    public float warningTime = 2f;

    private GameObject activeSeagull;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        float timeRemaining = playerMovement.timeBeforeSeagullDeath - playerMovement.GetTimeOutOfWater();

        if (timeRemaining <= warningTime && timeRemaining > 0 && !playerMovement.IsInWater())
        {
            if (activeSeagull == null)
            {
                SpawnSeagull();
            }
            else
            {
                MoveSeagullCloser();
            }
        }
        else
        {
            if (activeSeagull != null)
            {
                Destroy(activeSeagull);
            }
        }
    }

    void SpawnSeagull()
    {
        Vector3 spawnPos = transform.position + new Vector3(3f, 5f, 0f);
        activeSeagull = Instantiate(seagullPrefab, spawnPos, Quaternion.identity);
    }

    void MoveSeagullCloser()
    {
        Vector3 targetPos = transform.position + new Vector3(0f, 2f, 0f);
        activeSeagull.transform.position = Vector3.MoveTowards(
            activeSeagull.transform.position,
            targetPos,
            5f * Time.deltaTime
        );
    }
}
