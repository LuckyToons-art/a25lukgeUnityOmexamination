using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PixelDashMovement2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4f;

    [Header("Dash")]
    public float dashSpeed = 12f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.5f;

    [Header("Idle Bobbing")]
    public float bobAmplitude = 0.05f;
    public float bobFrequency = 3f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.down;

    private bool isDashing;
    private bool canDash = true;

    private TimeLimit timeLimit;
    private Vector3 startPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        timeLimit = FindObjectOfType<TimeLimit>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (isDashing) return;

        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        if (moveInput != Vector2.zero)
        {
            lastMoveDir = moveInput.normalized;
            startPosition = transform.position; // reset bob origin when moving
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        rb.linearVelocity = Vector2.ClampMagnitude(moveInput, 1f) * moveSpeed;

        if (moveInput == Vector2.zero)
        {
            IdleBob();
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        if (timeLimit != null)
            timeLimit.ConsumeDashTime();

        rb.linearVelocity = lastMoveDir * dashSpeed;
        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    public bool IsDashing() => isDashing;

    void IdleBob()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
        rb.MovePosition(new Vector2(rb.position.x, newY));
    }
}
