using UnityEngine;

[RequireComponent(typeof(Player2DMovement))]
public class FootstepAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] sandFootsteps;
    [SerializeField] private AudioClip[] waterFootsteps;
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private float footstepSpeed = 1.25f;

    private Player2DMovement player2DMovement;

    private float time;

    public void Start()
    {
        player2DMovement = GetComponent<Player2DMovement>();
    }

    public void Update()
    {
        if(player2DMovement.GetIsMoving())
        {
            time += Time.deltaTime * player2DMovement.GetCurrentSpeed() * footstepSpeed;

            if (time >= 1f)
            {
                time %= 1f;

                if (player2DMovement.GetIsInSea())
                {
                    int randomIndex = Random.Range(0, waterFootsteps.Length);
                    AudioClip footstepAudio = waterFootsteps[randomIndex];
                    footstepAudioSource.PlayOneShot(footstepAudio);
                }
                else 
                {
                    int randomIndex = Random.Range(0, sandFootsteps.Length);
                    AudioClip footstepAudio = sandFootsteps[randomIndex];
                    footstepAudioSource.PlayOneShot(footstepAudio);
                }
            }
        }
    }
}