using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameManager gameManager;

    void Start()
    {
        gameOverPanel.SetActive(false); // Hide game over screen initially
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        gameManager.EnableCursor();
        Time.timeScale = 0f; // Pause game
    }

    public void RestartGame()
    {
        Debug.Log("Restarting...");
        Time.timeScale = 1f; // Ensure time resumes
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}