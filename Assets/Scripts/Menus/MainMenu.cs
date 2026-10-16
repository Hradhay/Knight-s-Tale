using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public string gameSceneName;

    public void PlayGame()
    {
        // Reset time 
        Time.timeScale = 1f;

        // Reset player stats (singleton persists across scenes)
        if (StatsManager.Instance != null)
            StatsManager.Instance.ResetStats();

        // Load scene fresh every time
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quit button pressed. Exiting game...");
        Application.Quit();
    }
}