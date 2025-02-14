using UnityEngine;
using UnityEngine.AI;

public class GhostBehaviour : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 10f;
    public float killRadius = 1f;
    private NavMeshAgent navMeshAgent;
    private bool isChasing = false;
    private bool hasGameOverTriggered = false;
    public GameManager gameManager;
    // public LightManager lightManager; // Light mechanic removed

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the Ghost.");
        }

        // Ensure the ghost starts on a valid NavMesh position
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
            return;
        }

        bool isPlayerDetected = DetectPlayer();
        Debug.Log($"[Ghost] Detected: {isPlayerDetected} | Chasing: {isChasing}");

        if (isPlayerDetected)
        {
            if (!isChasing)
            {
                isChasing = true;
            }
            navMeshAgent.SetDestination(player.position);
        }
        else if (isChasing)
        {
            isChasing = false;
            navMeshAgent.ResetPath(); // Ghost stops moving when player escapes
        }

        // Check for game over condition
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= killRadius && !hasGameOverTriggered)
        {
            hasGameOverTriggered = true;
            Debug.Log("Player Caught! Game Over.");
            gameManager.GameOver();
        }

        if (!navMeshAgent.hasPath || navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
        {
            Debug.Log("[Ghost] No valid path or incomplete path!");
        }
        else
        {
            Debug.Log("[Ghost] Path is valid. Moving...");
        }
    }

    // 🔍 Detection using direct distance check
    bool DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool detected = distanceToPlayer <= detectionRadius;
        Debug.Log($"[Ghost] Player Detected? {detected} | Distance: {distanceToPlayer}");
        return detected;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, killRadius);
    }
}
