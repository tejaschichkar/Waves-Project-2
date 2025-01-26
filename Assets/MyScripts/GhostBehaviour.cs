using UnityEngine;
using UnityEngine.AI;

public class GhostBehaviour : MonoBehaviour
{
    // Reference to the player
    public Transform player;

    // Detection radius for the ghost to start following the player
    public float detectionRadius = 10f;

    // Reference to the NavMeshAgent component
    private NavMeshAgent navMeshAgent;

    // Flag to check if the ghost is following the player
    private bool isChasing = false;

    void Start()
    {
        // Get the NavMeshAgent component attached to the ghost
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Check if the NavMeshAgent exists
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the Ghost.");
        }
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
    }

    void Update()
    {
        if (player == null || navMeshAgent == null)
        {
            Debug.LogWarning("Player or NavMeshAgent is not assigned.");
            return;
        }

        // Calculate the distance between the ghost and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        Debug.Log("Distance to Player: " + distanceToPlayer);

        // Start following the player if within detection radius
        if (distanceToPlayer <= detectionRadius)
        {
            isChasing = true;
            Debug.Log("Player detected. Ghost is chasing.");
        }
        else
        {
            isChasing = false;
            Debug.Log("Player out of range. Ghost stopped.");
        }

        // Update the ghost's movement based on isChasing
        if (isChasing)
        {
            navMeshAgent.SetDestination(player.position);
            Debug.Log("Setting destination to Player.");
        }
        else
        {
            navMeshAgent.ResetPath(); // Stop moving
        }
    }

    // Debug visualization in the Scene view
    void OnDrawGizmosSelected()
    {
        // Draw the detection radius in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
