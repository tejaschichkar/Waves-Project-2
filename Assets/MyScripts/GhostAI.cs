using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float chaseSpeed = 3f;
    public float roamSpeed = 1.5f;
    public float roamRadius = 10f;
    public float stoppingDistance = 1.5f;
    public float stepHeight = 0.5f;
    public float stairClimbForce = 5f;
    public float rotationSpeed = 5f; // Smooth rotation speed

    private Vector3 roamTarget;
    private bool isChasing = false;
    private bool hasCapturedPlayer = false;
    private Rigidbody rb;

    public AudioSource chaseSound; // Assign in Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        FindNewRoamTarget();
    }

    void Update()
    {
        if (hasCapturedPlayer) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (!isChasing)
            {
                isChasing = true;
                if (chaseSound != null && !chaseSound.isPlaying)
                    chaseSound.Play(); // Start chase sound
            }
        }
        else if (distanceToPlayer > detectionRange * 1.2f)
        {
            if (isChasing)
            {
                isChasing = false;
                if (chaseSound != null && chaseSound.isPlaying)
                    chaseSound.Stop(); // Stop chase sound
            }
            FindNewRoamTarget();
        }

        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Roam();
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        // Smooth rotation instead of instant snapping
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (CheckForStairs(direction))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, stairClimbForce, rb.linearVelocity.z);
        }

        rb.MovePosition(transform.position + direction * chaseSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, player.position) <= stoppingDistance)
        {
            CapturePlayer();
        }
    }

    void Roam()
    {
        if (Vector3.Distance(transform.position, roamTarget) < 1f)
        {
            FindNewRoamTarget();
        }

        Vector3 direction = (roamTarget - transform.position).normalized;
        direction.y = 0;

        // Smooth rotation toward roam target
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (!Physics.Raycast(transform.position, direction, 1f))
        {
            rb.MovePosition(transform.position + direction * roamSpeed * Time.deltaTime);
        }
        else
        {
            FindNewRoamTarget();
        }
    }

    void FindNewRoamTarget()
    {
        roamTarget = transform.position + new Vector3(
            Random.Range(-roamRadius, roamRadius), 
            0, 
            Random.Range(-roamRadius, roamRadius)
        );
    }

    void CapturePlayer()
    {
        if (hasCapturedPlayer) return;

        hasCapturedPlayer = true;
        Debug.Log("Player Captured! Game Over.");
        if (chaseSound != null && chaseSound.isPlaying)
            chaseSound.Stop(); // Stop sound on game over
        GameManager.instance.GameOver();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CapturePlayer();
        }
    }

    bool CheckForStairs(Vector3 direction)
    {
        RaycastHit hit;
        Vector3 stairCheckPos = transform.position + direction * 0.5f + Vector3.up * 0.1f;

        if (Physics.Raycast(stairCheckPos, Vector3.down, out hit, stepHeight))
        {
            return hit.collider.CompareTag("Stairs");
        }
        return false;
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // Detection Range (Green)
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            // Stopping Distance (Red)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stoppingDistance);

            // Roam Area (Blue)
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, roamRadius);

            // Roam Target (Yellow)
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(roamTarget, 0.3f);

            // Forward Direction Line (Cyan)
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2f);
            
            // Path to Player (If Chasing)
            if (isChasing)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(transform.position, player.position);
            }
        }
    }
}
