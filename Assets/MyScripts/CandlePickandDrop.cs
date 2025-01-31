using UnityEngine;
using UnityEngine.UI;

public class CandlePickandDrop : MonoBehaviour
{
    // Total number of candles the player can carry
    private int totalCandles = 0;

    // Maximum candles the player can carry
    public int maxCandles = 6;

    // UI text for displaying the message
    public Text interactionText;

    // UI text for displaying candles collected
    public Text candlesCollectedText;

    // Message displayed after collecting all candles
    public string placeCandlesMessage = "Place all 6 candles in the ritual place to capture the ghost.";

    // Reference to the candle currently being interacted with
    private GameObject currentCandle;

    // Ritual place reference
    public GameObject ritualPlace;

    // Parent object containing 6 candles to place
    public GameObject ritualCandles;

    // Track if the candles have been placed
    private bool candlesPlaced = false;

    void Start()
    {
        // Ensure the interactionText and candlesCollectedText are hidden at the start
        if (interactionText != null)
        {
            interactionText.enabled = false;
        }

        if (candlesCollectedText != null)
        {
            candlesCollectedText.enabled = false;
        }
    }

    void Update()
    {
        // Check for player input to pick up the candle
        if (Input.GetKeyDown(KeyCode.E) && currentCandle != null)
        {
            PickUpCandle();
        }

        // Check for player input to place the candles at the ritual place
        if (Input.GetKeyDown(KeyCode.P) && candlesPlaced == false && totalCandles == maxCandles && IsNearRitualPlace())
        {
            PlaceCandles();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player collides with a candle
        if (other.CompareTag("Candle"))
        {
            // If the player hasn't reached the max candle limit
            if (totalCandles < maxCandles)
            {
                // Display the interaction message
                if (interactionText != null)
                {
                    interactionText.text = "Press E to pick up the candle";
                    interactionText.enabled = true;
                }

                // Set the current candle reference
                currentCandle = other.gameObject;
            }
        }

        // Check if the player collides with the ritual place
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
        // When the player exits the candle's collider, hide the interaction message
        if (other.CompareTag("Candle"))
        {
            if (interactionText != null)
            {
                interactionText.enabled = false;
            }

            // Clear the current candle reference
            currentCandle = null;
        }

        // When the player exits the ritual place's collider
        if (other.gameObject == ritualPlace)
        {
            if (interactionText != null)
            {
                interactionText.enabled = false;
            }
        }
    }

    void PickUpCandle()
    {
        // Increment the total candle count and remove the candle
        totalCandles++;
        Debug.Log("Picked up a candle. Total candles: " + totalCandles);

        // Disable the candle or destroy it
        currentCandle.SetActive(false);

        // Display candles collected text
        if (candlesCollectedText != null)
        {
            candlesCollectedText.enabled = true;
            candlesCollectedText.text = $"Candles Collected: {totalCandles}/{maxCandles}";

            // Show the place candles message when all candles are collected
            if (totalCandles == maxCandles)
            {
                candlesCollectedText.text = placeCandlesMessage;
            }
        }

        // Hide the interaction message
        if (interactionText != null)
        {
            interactionText.enabled = false;
        }

        // Clear the current candle reference
        currentCandle = null;
    }

    void PlaceCandles()
    {
        // Activate the parent object containing the candles
        if (ritualCandles != null)
        {
            ritualCandles.SetActive(true);
        }

        Debug.Log("Placed all candles at the ritual place.");

        // Hide the candles collected text
        if (candlesCollectedText != null)
        {
            candlesCollectedText.enabled = false;
        }

        // Mark candles as placed
        candlesPlaced = true;

        // Optionally, trigger further game logic here (e.g., capture the ghost).
    }

    bool IsNearRitualPlace()
    {
        // Check if the player is near the ritual place using a simple distance check
        return Vector3.Distance(transform.position, ritualPlace.transform.position) < 2.0f;
    }
}
