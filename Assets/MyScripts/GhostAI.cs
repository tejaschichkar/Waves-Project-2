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
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

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

        if (!Physics.Raycast(transform.position, direction, 1f))
        {
            rb.MovePosition(transform.position + direction * roamSpeed * Time.deltaTime);
        }
        else
        {
            FindNewRoamTarget();
        }

        transform.LookAt(new Vector3(roamTarget.x, transform.position.y, roamTarget.z));
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
}
