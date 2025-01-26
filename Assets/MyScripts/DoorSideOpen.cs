using UnityEngine;

public class DoorSideOpen : MonoBehaviour
{
    // Reference to the door GameObject
    public GameObject door;

    // Speed of the door opening (optional)
    public float openSpeed = 2f;

    // Rotation angles for opening the door
    private Quaternion leftOpenRotation;
    private Quaternion rightOpenRotation;
    private Quaternion closedRotation;

    // Flags to track if the door is opening or closing
    private bool isOpening = false;
    private bool isClosing = false;

    // Target rotation for the door
    private Quaternion targetRotation;

    void Start()
    {
        // Initialize the door's closed rotation
        if (door != null)
        {
            closedRotation = door.transform.rotation;

            // Set rotations for opening to the left and right
            leftOpenRotation = Quaternion.Euler(0, -90, 0) * closedRotation;
            rightOpenRotation = Quaternion.Euler(0, 90, 0) * closedRotation;
        }
    }

    void Update()
    {
        // Smoothly rotate the door to the target rotation if opening
        if ((isOpening || isClosing) && door != null)
        {
            door.transform.rotation = Quaternion.Lerp(
                door.transform.rotation,
                targetRotation,
                Time.deltaTime * openSpeed
            );

            // Stop moving the door when it reaches the target rotation
            if (Quaternion.Angle(door.transform.rotation, targetRotation) < 0.1f)
            {
                isOpening = false;
                isClosing = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player") && door != null)
        {
            // Determine the side of the player relative to the door
            Vector3 playerPosition = other.transform.position;
            Vector3 doorPosition = door.transform.position;

            // Check if the player is on the left or right side of the door
            if (playerPosition.x < doorPosition.x)
            {
                // Player is on the left side, open to the right
                targetRotation = rightOpenRotation;
            }
            else
            {
                // Player is on the right side, open to the left
                targetRotation = leftOpenRotation;
            }

            isOpening = true;
            isClosing = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger
        if (other.CompareTag("Player") && door != null)
        {
            // Close the door back to its original rotation
            targetRotation = closedRotation;
            isClosing = true;
            isOpening = false;
        }
    }
}
