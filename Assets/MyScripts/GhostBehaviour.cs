using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GhostBehaviour : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 10f; // Adjust as needed
    public float killRadius = 1.5f; // Adjust based on ghost size
    private NavMeshAgent navMeshAgent;
    private bool hasGameOverTriggered = false; // Prevent multiple GameOver calls
    public GameManager gameManager; // Reference to GameManager
    public LightManager lightManager; // Reference to LightManager
    public float roamTime = 5f; // Time to roam before choosing a new destination
    private bool isChasing = false; // Tracks if ghost is chasing

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the Ghost.");
        }

        StartCoroutine(RoamRandomly());
    }

    void Update()
    {
        if (player == null || navMeshAgent == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool isPlayerSafe = lightManager.IsPlayerSafe(player.position);

        // If player is in detection radius, start chasing
        if (distanceToPlayer <= detectionRadius)
        {
            isChasing = true;
            navMeshAgent.SetDestination(player.position);
        }
        else
        {
            isChasing = false; // If player escapes, return to roaming
        }

        // Only call GameOver if the player is inside kill radius and NOT in a lit area
        if (!isPlayerSafe && distanceToPlayer <= killRadius && !hasGameOverTriggered)
        {
            hasGameOverTriggered = true;
            Debug.Log("Player Caught! Game Over.");
            gameManager.GameOver();
        }
    }

    // Roaming logic (Ghost roams when not chasing)
    IEnumerator RoamRandomly()
    {
        while (true)
        {
            if (!isChasing) // Only pick a new roam point if NOT chasing
            {
                Vector3 randomDestination = GetValidRoamPoint();
                navMeshAgent.SetDestination(randomDestination);
            }

            yield return new WaitForSeconds(roamTime);
        }
    }

    // Get a random roam point that is NOT in a lit area
    Vector3 GetValidRoamPoint()
    {
        Vector3 randomPoint;
        NavMeshHit hit;

        do
        {
            randomPoint = Random.insideUnitSphere * 10f + transform.position;
        } while (!NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas) || lightManager.IsPositionSafe(hit.position));

        return hit.position;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, killRadius);
    }
}
