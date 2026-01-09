using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class ChasingEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;

    [Header("Chase Settings")]
    public float chaseSpeed = 3f;
    public float detectionRadius = 5f;

    [Header("Health")]
    public int maxHealth = 1;

    [Header("Time Reward")]
    public int timeReward = 10; // seconds added on death

    [Header("Damage on Contact")]
    public float damageInterval = 0.5f; // seconds between timer drain
    public float damageAmount = 1f;      // amount of time drained

    private Rigidbody2D rb;
    private Vector2 targetPos;
    private int currentHealth;

    private Transform player;
    private TimeLimit timeLimit;

    private float damageTimer = 0f;
    private bool playerInContact = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        currentHealth = maxHealth;

        if (!pointA || !pointB)
            Debug.LogError("ChasingEnemy: Assign pointA and pointB!");

        targetPos = pointB.position;

        player = FindObjectOfType<PixelDashMovement2D>()?.transform;
        timeLimit = FindObjectOfType<TimeLimit>();
    }

    void FixedUpdate()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) <= detectionRadius)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        HandleContactDamage();
    }

    #region Patrol
    void Patrol()
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, patrolSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector2.Distance(rb.position, targetPos) < 0.05f)
            targetPos = targetPos == (Vector2)pointA.position ? pointB.position : pointA.position;
    }
    #endregion

    #region Chase
    void ChasePlayer()
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, player.position, chaseSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }
    #endregion

    #region Damage
    void HandleContactDamage()
    {
        if (!playerInContact || timeLimit == null) return;

        damageTimer += Time.fixedDeltaTime;
        if (damageTimer >= damageInterval)
        {
            timeLimit.ConsumeDashTime(); // we can also subtract directly
            timeLimit.AddTime(-damageAmount); // subtract damageAmount from timer
            damageTimer = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<PixelDashMovement2D>() != null)
        {
            playerInContact = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<PixelDashMovement2D>() != null)
        {
            playerInContact = false;
            damageTimer = 0f;
        }
    }
    #endregion

    #region Damage from Dash
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (timeLimit != null)
            timeLimit.AddTime(timeReward);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detect player dash
        PixelDashMovement2D playerDash = collision.GetComponent<PixelDashMovement2D>();
        if (playerDash != null && playerDash.IsDashing())
        {
            TakeDamage(1);
        }
    }
    #endregion
}
