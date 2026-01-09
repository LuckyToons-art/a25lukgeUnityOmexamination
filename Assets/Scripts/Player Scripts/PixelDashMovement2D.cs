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

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDir = Vector2.down;

    private bool isDashing;
    private bool canDash = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isDashing) return;

        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        if (moveInput != Vector2.zero)
            lastMoveDir = moveInput.normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        rb.linearVelocity = lastMoveDir * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
