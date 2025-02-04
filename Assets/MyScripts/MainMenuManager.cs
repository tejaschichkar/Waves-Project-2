using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject startPanel;    // Reference to Start Panel
    public GameObject howToPlayPanel; // Reference to How To Play Panel

    void Start()
    {
        // Ensure the Start Panel is active & How To Play is hidden
        startPanel.SetActive(true);
        howToPlayPanel.SetActive(false);
    }

    public void PlayGame()
    {
        startPanel.SetActive(false); // Hide the Start Panel
        SceneManager.LoadScene(1); // Load Game Scene
    }

    public void ShowHowToPlay()
    {
        startPanel.SetActive(false);  // Hide Start Panel
        howToPlayPanel.SetActive(true); // Show How To Play Panel
    }

    public void BackToMenu()
    {
        howToPlayPanel.SetActive(false); // Hide How To Play Panel
        startPanel.SetActive(true); // Show Start Panel
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit Application
        Debug.Log("Game Quit");
    }
}
