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

    // Layer mask to detect the floor
    public LayerMask floorLayer;

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

            // Adjust target position to align with the floor
            Vector3 adjustedPosition = AdjustToFloor(targetPoint.position);
            targetPoint.position = adjustedPosition;

            // Start moving the ghost
            shouldMove = true;
        }
    }

    Vector3 AdjustToFloor(Vector3 targetPosition)
    {
        // Cast a ray downward from the target position to find the floor
        RaycastHit hit;
        if (Physics.Raycast(targetPosition + Vector3.up, Vector3.down, out hit, Mathf.Infinity, floorLayer))
        {
            // Adjust the y-coordinate to the floor level
            targetPosition.y = hit.point.y;
        }
        else
        {
            Debug.LogWarning("Floor not detected for target position. Ghost might remain in mid-air.");
        }

        return targetPosition;
    }
}