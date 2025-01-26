using UnityEngine;
using UnityEngine.UI; // Include if you use UI elements like Text

public class CandlePickandDrop : MonoBehaviour
{
    // Total number of candles the player can carry
    private int totalCandles = 0;

    // Maximum candles the player can carry
    public int maxCandles = 6;

    // UI text for displaying the message
    public Text interactionText;

    // Reference to the candle currently being interacted with
    private GameObject currentCandle;

    void Start()
    {
        // Ensure the interactionText is hidden at the start
        if (interactionText != null)
        {
            interactionText.enabled = false;
        }
    }

    void Update()
    {
        // Check for player input to pick up the candle
        if (Input.GetKeyDown(KeyCode.E) && currentCandle != null)
        {
            PickUpCandle();
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
    }

    void PickUpCandle()
    {
        // Increment the total candle count and remove the candle
        totalCandles++;
        Debug.Log("Picked up a candle. Total candles: " + totalCandles);

        // Disable the candle or destroy it
        currentCandle.SetActive(false);

        // Hide the interaction message
        if (interactionText != null)
        {
            interactionText.enabled = false;
        }

        // Clear the current candle reference
        currentCandle = null;
    }
}
