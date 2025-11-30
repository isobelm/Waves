using UnityEngine;

public class StartButton : MonoBehaviour
{

    GameStateController gameStateController;
    void Start()
    {
        gameStateController = FindFirstObjectByType<GameStateController>();
    }

	void OnMouseUp()
	{
        gameStateController.StartGame();
	}
}
