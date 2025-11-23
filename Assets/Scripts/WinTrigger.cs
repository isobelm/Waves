using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameStateController gameStateController = FindFirstObjectByType<GameStateController>();
            gameStateController.PlayerWon();
        }
    }
}
