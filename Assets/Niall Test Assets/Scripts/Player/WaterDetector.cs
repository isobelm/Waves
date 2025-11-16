using UnityEngine;
using UnityEngine.Events;

public class WaterDetector : MonoBehaviour
{
    [Header("Water Detection")]
    [Tooltip("Event fired when player enters water")]
    public UnityEvent OnEnterWater;

    [Tooltip("Event fired when player exits water")]
    public UnityEvent OnExitWater;
    
    private bool isInWater = false;
    
    public bool IsInWater() => isInWater;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            OnEnterWater?.Invoke();
            Debug.Log("Entered water");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            OnExitWater?.Invoke();
            Debug.Log("Exited water");
        }
    }
}