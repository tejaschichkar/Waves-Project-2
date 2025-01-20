using UnityEngine;

public class DoorOpen1 : MonoBehaviour
{
    // Reference to the door GameObject that will open
    public GameObject doorToOpen;

    // Speed of the door opening (optional, for smooth animations)
    public float openSpeed = 2f;

    // The target rotation for the door to simulate opening
    private Quaternion targetRotation;

    // Flag to track if the door is opening
    private bool isOpening = false;

    void Start()
    {
        // Set the target rotation (for example, 90 degrees around the Y axis)
        if (doorToOpen != null)
        {
            targetRotation = Quaternion.Euler(0, 90, 0) * doorToOpen.transform.rotation;
        }
    }

    void Update()
    {
        // Smoothly rotate the door if it is opening
        if (isOpening && doorToOpen != null)
        {
            doorToOpen.transform.rotation = Quaternion.Lerp(
                doorToOpen.transform.rotation,
                targetRotation,
                Time.deltaTime * openSpeed
            );

            // Stop the animation if the door has reached its target rotation
            if (Quaternion.Angle(doorToOpen.transform.rotation, targetRotation) < 0.1f)
            {
                isOpening = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.CompareTag("Player") && doorToOpen != null)
        {
            isOpening = true;
        }
    }
}
