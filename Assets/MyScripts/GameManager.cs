using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameOverPanel; // Assign in Inspector

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameOverPanel.SetActive(false); // Hide Game Over UI at start
    }

    public void GameOver()
    {
        Debug.Log("Game Over! Player caught.");
        gameOverPanel.SetActive(true); // Show Game Over UI
        FindObjectOfType<CharController_Motor>().EnableCursor();
        Time.timeScale = 0f; // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume game
        Debug.Log("Time Scale After Reset: " + Time.timeScale);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}