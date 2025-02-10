using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GhostBehaviour : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 10f;
    public float killRadius = 1f; // Adjust based on ghost size
    private NavMeshAgent navMeshAgent;
    private bool isChasing = false;
    private bool hasGameOverTriggered = false; // Prevent multiple GameOver calls
    public GameManager gameManager; // ✅ Reference to GameManager
    public LightManager lightManager; // Reference to LightManager
    public float roamTime = 5f; // Time to roam before choosing a new destination

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the Ghost.");
        }

        // Snap ghost to closest NavMesh position if it's off-mesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }

        StartCoroutine(RoamRandomly());
    }

    void Update()
    {
        if (player == null || navMeshAgent == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            navMeshAgent.ResetPath();
        }

        // Only call GameOver once
        if (distanceToPlayer <= killRadius && !hasGameOverTriggered)
        {
            hasGameOverTriggered = true; // Prevent multiple calls
            Debug.Log("Player Caught! Game Over.");
            gameManager.GameOver();
        }
    }

    // Roaming logic
    IEnumerator RoamRandomly()
    {
        while (true)
        {
            if (!isChasing) // Only roam when not chasing
            {
                // Find a random position within the NavMesh bounds
                Vector3 randomDestination = GetRandomNavMeshPoint();

                // Check if the random destination is near any active light
                while (lightManager.IsPlayerSafe(randomDestination))
                {
                    randomDestination = GetRandomNavMeshPoint(); // If near light, pick another point
                }

                navMeshAgent.SetDestination(randomDestination); // Set the new destination
            }

            // Wait for the roam time before choosing a new destination
            yield return new WaitForSeconds(roamTime);
        }
    }

    // Get a random point within the NavMesh
    Vector3 GetRandomNavMeshPoint()
    {
        Vector3 randomPoint = Random.insideUnitSphere * 10f; // Adjust range as necessary
        randomPoint += transform.position; // Add the current position as the center

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas)) // Adjust range as necessary
        {
            return hit.position;
        }

        return transform.position; // Return current position if no valid point found
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, killRadius);
    }
}