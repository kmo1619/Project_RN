using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerController controller;

    private Rigidbody2D rb;

    [Header("Ground Check")]
    public Transform groundCheck;

    public LayerMask groundLayer;

    public float groundRadius = 0.2f;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        IsGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer);
    }

    public void Move(float input)
    {
        rb.linearVelocity = new Vector2(
            input * controller.stats.moveSpeed,
            rb.linearVelocity.y);
    }

    public void Stop()
    {
        rb.linearVelocity = new Vector2(
            0,
            rb.linearVelocity.y);
    }

    public void Jump()
    {
        if (!IsGrounded)
            return;

        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            controller.stats.jumpForce);
    }

    public void Flip(float input)
    {
        if (input == 0)
            return;

        Vector3 scale = transform.localScale;

        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(input);

        transform.localScale = scale;
    }
}