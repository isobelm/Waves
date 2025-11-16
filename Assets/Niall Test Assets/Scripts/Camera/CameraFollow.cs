using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Settings")]
    [Range(0.01f, 1f)]
    [Tooltip("How smoothly the camera follows the player (lower = smoother)")]
    public float smoothSpeed = 0.125f;

    [Tooltip("Offset from the player's position")]
    public Vector3 offset = new Vector3(0, 0, -10);

    [Tooltip("The player transform to follow")]
    public Transform player;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
