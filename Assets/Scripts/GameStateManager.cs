using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject game;

    public void StartGame()
    {
        mainMenu.SetActive(false);
        game.SetActive(true);
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        game.SetActive(false);
        mainMenu.SetActive(true);
        Time.timeScale = 1f;
    }
}