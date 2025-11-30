using UnityEngine;

[RequireComponent(typeof(GameObject))]
public class FootstepAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private GameObject player;

    [SerializeField] private float footstepSpeed = 1;

    private Player2DMovement player2DMovement;

    private float time;

    public void Start()
    {
        player2DMovement = player.GetComponent<Player2DMovement>();
    }

    public void Update()
    {
        if(player2DMovement.GetIsMoving())
        {
            time += Time.deltaTime * player2DMovement.GetCurrentSpeed() * footstepSpeed;

            if (time >= 1f)
            {
                time %= 1f;

                int randomIndex = Random.Range(0, footsteps.Length);
                AudioClip footstepAudio = footsteps[randomIndex];
                footstepAudioSource.PlayOneShot(footstepAudio);
            }
        }
    }
}