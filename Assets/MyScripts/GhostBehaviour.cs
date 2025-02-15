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

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (navMeshAgent == null)
        {
            Debug.LogError("[Ghost] NavMeshAgent component is missing.");
            return;
        }

        // Auto-assign player if not set in Inspector
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogError("[Ghost] Player not found! Ensure player has 'Player' tag.");
        }

        // Ensure Ghost starts on NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            navMeshAgent.Warp(hit.position);
        }
        else
        {
            Debug.LogError("[Ghost] Failed to place Ghost on NavMesh!");
        }
    }

    void Update()
    {
        if (player == null || navMeshAgent == null)
            return;

        bool isPlayerDetected = DetectPlayer();
        Debug.Log($"[Ghost] Detected: {isPlayerDetected} | Chasing: {isChasing}");

        if (isPlayerDetected)
        {
            if (!isChasing)
                isChasing = true;

            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(player.position);

            // Check if path is valid
            if (navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                Debug.LogError("[Ghost] Invalid path! Can't reach player.");
                return;
            }
        }
        else if (isChasing)
        {
            isChasing = false;
            navMeshAgent.ResetPath(); // Stop chasing
        }

        // Check for Game Over condition
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= killRadius && !hasGameOverTriggered)
        {
            hasGameOverTriggered = true;
            Debug.Log("Player Caught! Game Over.");
            gameManager.GameOver();
        }

        // Debug NavMeshAgent movement
        Debug.Log($"[Ghost] Speed: {navMeshAgent.velocity.magnitude} | Path Status: {navMeshAgent.pathStatus}");
    }

    // Detect player using direct distance check
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
