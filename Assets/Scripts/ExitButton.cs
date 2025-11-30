using UnityEngine;

public class ExitButton : MonoBehaviour
{

    GameStateController gameStateController;
    void Start()
    {
        gameStateController = FindFirstObjectByType<GameStateController>();
    }

	void OnMouseUp()
	{
        gameStateController.ExitGame();
	}
}
