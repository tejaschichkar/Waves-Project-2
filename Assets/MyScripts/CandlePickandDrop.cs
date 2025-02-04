using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CandlePickandDrop : MonoBehaviour
{
    private int totalCandles = 0;
    public int maxCandles = 6;

    public Text interactionText;
    public Text candlesCollectedText;
    public string placeCandlesMessage = "Place all 6 candles in the ritual place to capture the ghost.";

    private GameObject currentCandle;
    public GameObject ritualPlace;
    public GameObject ritualCandles;

    private bool candlesPlaced = false;

    // Win Screen UI Panel
    public GameObject winScreen;

    void Start()
    {
        if (interactionText != null) interactionText.enabled = false;
        if (candlesCollectedText != null) candlesCollectedText.enabled = false;
        if (ritualCandles != null) ritualCandles.SetActive(false);

        // Ensure Win Screen is disabled at the start
        if (winScreen != null) winScreen.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentCandle != null)
        {
            PickUpCandle();
        }

        if (Input.GetKeyDown(KeyCode.P) && !candlesPlaced && totalCandles == maxCandles && IsNearRitualPlace())
        {
            PlaceCandles();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Candle") && totalCandles < maxCandles)
        {
            if (interactionText != null)
            {
                interactionText.text = "Press E to pick up the candle";
                interactionText.enabled = true;
            }
            currentCandle = other.gameObject;
        }

        if (other.gameObject == ritualPlace && totalCandles == maxCandles)
        {
            if (interactionText != null)
            {
                interactionText.text = "Press P to place the candles";
                interactionText.enabled = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Candle"))
        {
            if (interactionText != null) interactionText.enabled = false;
            currentCandle = null;
        }

        if (other.gameObject == ritualPlace)
        {
            if (interactionText != null) interactionText.enabled = false;
        }
    }

    void PickUpCandle()
    {
        totalCandles++;
        Debug.Log("Picked up a candle. Total candles: " + totalCandles);
        currentCandle.SetActive(false);
        currentCandle = null;

        if (candlesCollectedText != null)
        {
            candlesCollectedText.enabled = true;
            candlesCollectedText.text = $"Candles Collected: {totalCandles}/{maxCandles}";

            if (totalCandles == maxCandles)
            {
                candlesCollectedText.text = placeCandlesMessage;
            }
        }

        if (interactionText != null) interactionText.enabled = false;
    }

    void PlaceCandles()
    {
        if (ritualCandles != null)
        {
            ritualCandles.SetActive(true);
        }

        Debug.Log("Placed all candles at the ritual place.");
        candlesPlaced = true;

        if (candlesCollectedText != null) candlesCollectedText.enabled = false;
        if (interactionText != null) interactionText.enabled = false;

        // 🎉 Show the Win Screen 🎉
        if (winScreen != null)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    bool IsNearRitualPlace()
    {
        if (ritualPlace == null) return false;
        return Vector3.Distance(transform.position, ritualPlace.transform.position) < 2.0f;
    }

    // 💡 Called when "Play Again" Button is Clicked
    public void PlayAgain()
    {
        Time.timeScale = 1f; // Unpause the game
        SceneManager.LoadScene(0); // Reloads the scene
    }

    // 💡 Called when "Quit" Button is Clicked
    public void QuitGame()
    {
        Application.Quit(); // Closes the game
        Debug.Log("Game Quit");
    }
}