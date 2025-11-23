using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new(0, 0, -10);
    
    public float minX = -10f;
    public float maxX = 10f;
    
    public float fixedY = 0f;
    
    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        
        float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
        
        desiredPosition = new Vector3(clampedX, fixedY, desiredPosition.z);
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}