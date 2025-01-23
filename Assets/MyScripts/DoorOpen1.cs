using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    // Reference to the door GameObject that will open/close
    public GameObject doorToOpen;

    // Speed of the door opening/closing
    public float moveSpeed = 2f;

    // The target rotations for the door (open and closed states)
    private Quaternion openRotation;
    private Quaternion closedRotation;

    // Flag to track if the door is opening or closing
    private bool isOpening = false;
    private bool isClosing = false;

    void Start()
    {
        // Initialize the door's open and closed rotations
        if (doorToOpen != null)
        {
            closedRotation = doorToOpen.transform.rotation;
            openRotation = Quaternion.Euler(0, 90, 0) * closedRotation;
        }
    }

    void Update()
    {
        // Smoothly rotate the door if it is opening
        if (isOpening && doorToOpen != null)
        {
            doorToOpen.transform.rotation = Quaternion.Lerp(
                doorToOpen.transform.rotation,
                openRotation,
                Time.deltaTime * moveSpeed
            );

            // Stop the animation when the door reaches the open position
            if (Quaternion.Angle(doorToOpen.transform.rotation, openRotation) < 0.1f)
            {
                isOpening = false;
            }
        }

        // Smoothly rotate the door if it is closing
        if (isClosing && doorToOpen != null)
        {
            doorToOpen.transform.rotation = Quaternion.Lerp(
                doorToOpen.transform.rotation,
                closedRotation,
                Time.deltaTime * moveSpeed
            );

            // Stop the animation when the door reaches the closed position
            if (Quaternion.Angle(doorToOpen.transform.rotation, closedRotation) < 0.1f)
            {
                isClosing = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player") && doorToOpen != null)
        {
            isOpening = true;
            isClosing = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player exited the trigger
        if (other.CompareTag("Player") && doorToOpen != null)
        {
            isClosing = true;
            isOpening = false;
        }
    }
}