using UnityEngine;

public class GhostPositioned : MonoBehaviour
{
    // Reference to the ghost GameObject
    public GameObject ghost;

    // Array of possible points where the ghost can move
    public Transform[] targetPositions;

    // Optional speed for smooth movement (set to 0 for instant teleportation)
    public float moveSpeed = 0f;

    // Flag to track if the ghost should move
    private bool shouldMove = false;

    // The target position for the ghost
    private Transform targetPoint;

    void Update()
    {
        // Smoothly move the ghost to the target position if it should move
        if (shouldMove && ghost != null && targetPoint != null)
        {
            if (moveSpeed > 0)
            {
                // Smooth movement
                ghost.transform.position = Vector3.MoveTowards(
                    ghost.transform.position,
                    targetPoint.position,
                    moveSpeed * Time.deltaTime
                );

                // Stop moving if the ghost reaches the target
                if (Vector3.Distance(ghost.transform.position, targetPoint.position) < 0.1f)
                {
                    shouldMove = false;
                }
            }
            else
            {
                // Instant teleportation
                ghost.transform.position = targetPoint.position;
                shouldMove = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the object
        if (other.CompareTag("Player") && ghost != null && targetPositions.Length > 0)
        {
            // Select a random point from the array of target positions
            int randomIndex = Random.Range(0, targetPositions.Length);
            targetPoint = targetPositions[randomIndex];

            // Start moving the ghost
            shouldMove = true;
        }
    }
}
