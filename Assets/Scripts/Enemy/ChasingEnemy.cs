using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(AudioSource))]
public class ChasingEnemy : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform pointA;
    public Transform pointB;
    public float patrolSpeed = 2f;

    [Header("Chase Settings")]
    public float chaseSpeed = 3f;
    public float detectionRadius = 5f;

    [Header("Health & Time Reward")]
    public int maxHealth = 1;
    public int timeReward = 10;

    [Header("Contact Damage")]
    public float damageInterval = 0.5f;
    public float damageAmount = 1f;

    [Header("Audio")]
    public AudioClip detectionSound;

    private Rigidbody2D rb;
    private Vector2 targetPos;
    private int currentHealth;

    private Transform player;
    private TimeLimit timeLimit;
    private AudioSource audioSource;

    private float damageTimer = 0f;
    private bool playerInContact = false;
    private bool hasDetectedPlayer = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        currentHealth = maxHealth;

        if (!pointA || !pointB) Debug.LogError("Assign patrol points!");

        targetPos = pointB.position;
        player = FindObjectOfType<PixelDashMovement2D>()?.transform;
        timeLimit = FindObjectOfType<TimeLimit>();

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= detectionRadius)
            {
                if (!hasDetectedPlayer)
                {
                    PlayDetectionSound();
                    hasDetectedPlayer = true;
                }
                ChasePlayer();
            }
            else
            {
                hasDetectedPlayer = false;
                Patrol();
            }
        }
        else
        {
            Patrol();
        }

        HandleContactDamage();
    }

    void Patrol()
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, patrolSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector2.Distance(rb.position, targetPos) < 0.05f)
            targetPos = targetPos == (Vector2)pointA.position ? pointB.position : pointA.position;
    }

    void ChasePlayer()
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, player.position, chaseSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    void PlayDetectionSound()
    {
        if (audioSource != null && detectionSound != null)
            audioSource.PlayOneShot(detectionSound);
    }

    void HandleContactDamage()
    {
        if (!playerInContact || timeLimit == null) return;

        damageTimer += Time.fixedDeltaTime;
        if (damageTimer >= damageInterval)
        {
            timeLimit.AddTime(-damageAmount);
            damageTimer = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<PixelDashMovement2D>() != null)
            playerInContact = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<PixelDashMovement2D>() != null)
        {
            playerInContact = false;
            damageTimer = 0f;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (timeLimit != null) timeLimit.AddTime(timeReward);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PixelDashMovement2D playerDash = collision.GetComponent<PixelDashMovement2D>();
        if (playerDash != null && playerDash.IsDashing())
        {
            TakeDamage(1);
        }
    }
}
