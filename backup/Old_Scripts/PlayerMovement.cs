using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // =========================
    // 이동 설정
    // =========================
    [Header("Movement")]
    public float moveSpeed = 5f; // 플레이어 이동 속도

    // =========================
    // 점프 설정
    // =========================
    [Header("Jump")]
    public float jumpForce = 10f; // 점프 힘

    // =========================
    // 바닥 체크 설정
    // =========================
    [Header("Ground Check")]
    public Transform groundCheck; // 바닥 체크 위치
    public LayerMask groundLayer; // 바닥으로 인식할 레이어

    private Rigidbody2D rb;
    private bool isGrounded; // 현재 바닥에 닿아있는지 여부

    // =========================
    // 대시 설정
    // =========================
    public float dashDistance = 3f; // 대시 이동 거리

    void Start()
    {
        // Rigidbody2D 컴포넌트 가져오기
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // =========================
        // 바닥 판정
        // groundCheck 위치를 중심으로
        // 반지름 0.2짜리 원을 생성하여
        // groundLayer와 충돌하는지 검사
        // =========================
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            0.2f,
            groundLayer
        );

        // =========================
        // 좌우 이동 입력
        // =========================
        float horizontal = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal = -1; // 왼쪽 이동
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 1; // 오른쪽 이동
        }

        // =========================
        // 실제 이동 처리
        // transform.position을 직접 변경
        // =========================
        transform.position += new Vector3(
            horizontal,
            0,
            0
        ) * moveSpeed * Time.deltaTime;

        // =========================
        // 캐릭터 방향 전환
        // scale.x를 뒤집어서 좌우 반전
        // =========================
        if (horizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // =========================
        // 점프
        // 스페이스를 누르고
        // 바닥에 있을 때만 점프
        // =========================
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );
        }

        // =========================
        // 대시
        // LeftShift를 누르면
        // 바라보는 방향으로 순간이동
        // =========================
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // scale.x가
            // 오른쪽이면 1
            // 왼쪽이면 -1
            float direction = transform.localScale.x;

            transform.position += Vector3.right * direction * dashDistance;
        }
    }
}