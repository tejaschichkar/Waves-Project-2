using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    public GameObject gameOverPanel; // Assign in the Inspector

    void Start()
    {
        gameOverPanel.SetActive(false); // Hide game over screen initially
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ghost")) // Make sure Ghost has the "Ghost" tag
        {
            Debug.Log("Game Over! Player caught by Ghost.");
            GameOver();
        }
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true); // Show game over screen
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume game time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload level
         gameOverPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
