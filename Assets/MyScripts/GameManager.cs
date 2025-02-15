using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameOverPanel; // Assign in Inspector
    public GameObject pausePanel;    // Assign PausePanel in Inspector
    private bool isPaused = false;

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
        pausePanel.SetActive(false);    // Hide Pause Panel at start
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f; // Pause the game
        Debug.Log("Game Over! Player caught.");
        gameOverPanel.SetActive(true); // Show Game Over UI
        EnableCursor();
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

    // PAUSE MENU FUNCTIONALITY
    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        EnableCursor();
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        DisableCursor();
        Time.timeScale = 1f;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Reset time before switching scene
        SceneManager.LoadScene(0); // Loads Main Menu (Scene Index 0)
    }

    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Call this when the game ends (Game Over / Win)
    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
