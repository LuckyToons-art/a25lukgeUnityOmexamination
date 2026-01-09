using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PatrollingEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;

    [Header("Health")]
    public int maxHealth = 1;

    [Header("Time Reward")]
    public int timeReward = 10; // seconds added on death

    private Rigidbody2D rb;
    private Vector2 targetPos;
    private int currentHealth;

    private TimeLimit timeLimit;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        currentHealth = maxHealth;

        if (!pointA || !pointB)
        {
            Debug.LogError("PatrollingEnemy: Assign pointA and pointB in inspector!");
        }

        targetPos = pointB.position;

        timeLimit = FindObjectOfType<TimeLimit>();
    }

    void FixedUpdate()
    {
        Patrol();
    }

    void Patrol()
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector2.Distance(rb.position, targetPos) < 0.05f)
        {
            targetPos = targetPos == (Vector2)pointA.position ? pointB.position : pointA.position;
        }
    }

    // Call this when player dashes through the enemy
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Reward player by increasing timer
        if (timeLimit != null)
        {
            timeLimit.AddTime(timeReward);
        }

        Destroy(gameObject);
    }

    // Optional: trigger via collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detect dash (assuming player has a "Player" tag and is dashing)
        PixelDashMovement2D playerDash = collision.GetComponent<PixelDashMovement2D>();
        if (playerDash != null && playerDash.IsDashing())
        {
            TakeDamage(1);
        }
    }
}
