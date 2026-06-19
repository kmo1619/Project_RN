using UnityEngine;

public enum WallSide
{
    None,
    Left,
    Right
}

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerController controller;

    private Rigidbody2D rb;

    private BoxCollider2D col;

    [Header("Ground Check")]
    public Transform groundCheck;

    public LayerMask groundLayer;

    public float groundRadius = 0.2f;

    [Header("Wall Check")]
    public float wallCheckDistance = 0.05f;

    public bool IsGrounded { get; private set; }

    public WallSide CurrentWallSide { get; private set; }

    public bool IsWallLeft => CurrentWallSide == WallSide.Left;

    public bool IsWallRight => CurrentWallSide == WallSide.Right;

    public Vector2 WallNormal { get; private set; }

    private float desiredMoveInput;

    private bool jumpRequested;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        CheckGround();
        CheckWalls();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void CheckGround()
    {
        IsGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundRadius,
            groundLayer);
    }

    private void CheckWalls()
    {
        Vector2 origin  = col.bounds.center;
        Vector2 boxSize = col.bounds.size * 0.95f;

        RaycastHit2D hitLeft = Physics2D.BoxCast(
            origin,
            boxSize,
            0f,
            Vector2.left,
            wallCheckDistance,
            groundLayer);

        RaycastHit2D hitRight = Physics2D.BoxCast(
            origin,
            boxSize,
            0f,
            Vector2.right,
            wallCheckDistance,
            groundLayer);

        Debug.DrawRay(
            origin + Vector2.left  * (boxSize.x * 0.5f),
            Vector2.left  * wallCheckDistance,
            hitLeft  ? Color.green : Color.red);

        Debug.DrawRay(
            origin + Vector2.right * (boxSize.x * 0.5f),
            Vector2.right * wallCheckDistance,
            hitRight ? Color.green : Color.red);

        if (hitLeft)
        {
            CurrentWallSide = WallSide.Left;
            WallNormal      = hitLeft.normal;
        }
        else if (hitRight)
        {
            CurrentWallSide = WallSide.Right;
            WallNormal      = hitRight.normal;
        }
        else
        {
            CurrentWallSide = WallSide.None;
            WallNormal      = Vector2.zero;
        }

#if UNITY_EDITOR
        if (desiredMoveInput != 0f)
        {
            Debug.Log(
                $"[WallCheck] " +
                $"WallSide={CurrentWallSide} WallNormal={WallNormal} | " +
                $"vel=({rb.linearVelocity.x:F4}, {rb.linearVelocity.y:F4})");
        }
#endif
    }

    private void ApplyMovement()
    {
        float moveX = desiredMoveInput * controller.stats.moveSpeed;

        if (desiredMoveInput > 0f && IsWallRight)
            moveX = 0f;
        else if (desiredMoveInput < 0f && IsWallLeft)
            moveX = 0f;

        rb.linearVelocity = new Vector2(moveX, rb.linearVelocity.y);

        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                controller.stats.jumpForce);

            jumpRequested = false;
        }
    }

    public void Move(float input)
    {
        desiredMoveInput = input;
    }

    public void Stop()
    {
        desiredMoveInput = 0f;
    }

    public void Jump()
    {
        if (!IsGrounded)
            return;

        jumpRequested = true;
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
