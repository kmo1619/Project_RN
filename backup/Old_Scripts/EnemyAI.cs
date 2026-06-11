using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    // =========================
    // 플레이어 타겟
    // =========================
    [Header("Target")]
    public Transform player;

    // =========================
    // 공격 설정
    // =========================
    [Header("Attack")]
    public float attackRange = 1.2f;      // 공격 가능 거리
    public int attackDamage = 10;         // 공격 데미지
    public float attackCooldown = 1f;     // 공격 후 쿨타임

    // =========================
    // 공격 전조 시간
    // =========================
    [Header("Attack Timing")]
    public float attackDelay = 0.7f;      // 공격 전 대기 시간

    // =========================
    // 이동 설정
    // =========================
    [Header("Movement")]
    public float moveSpeed = 2f;          // 추적 이동 속도

    // =========================
    // 플레이어 감지 범위
    // =========================
    [Header("Detection")]
    public float detectionRange = 5f;     // 플레이어 탐지 거리

    // 공격 진행 여부
    private bool isAttacking;

    // 공격 전조 색상 변경용
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        // SpriteRenderer 가져오기
        spriteRenderer =
            GetComponent<SpriteRenderer>();

        // 원래 색상 저장
        if (spriteRenderer != null)
        {
            originalColor =
                spriteRenderer.color;
        }
    }

    void Update()
    {
        // 플레이어가 없으면 종료
        if (player == null)
            return;

        // 적 전투 상태 확인
        EnemyCombat enemyCombat =
            GetComponent<EnemyCombat>();

        if (enemyCombat != null)
        {
            // 경직 상태면 행동 불가
            if (enemyCombat.IsHitStunned())
                return;

            // 넉다운 상태면 행동 불가
            if (enemyCombat.IsKnockedDown())
                return;
        }

        // 공격 중이면 다른 행동 금지
        if (isAttacking)
            return;

        // 플레이어와의 거리 계산
        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        // 플레이어 방향 바라보기
        if (player.position.x >
            transform.position.x)
        {
            transform.localScale =
                new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale =
                new Vector3(-1, 1, 1);
        }

        // 공격 범위 안에 있으면 공격 시작
        if (distance <= attackRange)
        {
            StartCoroutine(
                AttackRoutine()
            );
        }
        // 감지 범위 안에 있으면 추적
        else if (distance <= detectionRange)
        {
            FollowPlayer();
        }
    }

    // =========================
    // 플레이어 추적
    // =========================
    void FollowPlayer()
    {
        // 플레이어의 X좌표만 추적
        Vector3 targetPosition =
            new Vector3(
                player.position.x,
                transform.position.y,
                transform.position.z
            );

        // 플레이어 방향으로 이동
        transform.position =
            Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
    }

    // =========================
    // 공격 전조 → 공격 → 쿨타임
    // =========================
    IEnumerator AttackRoutine()
    {
        // 공격 시작
        isAttacking = true;

        Debug.Log("Attack Ready!");

        // 공격 전조 색상 변경
        if (spriteRenderer != null)
        {
            spriteRenderer.color =
                Color.yellow;
        }

        // 전조 시간 대기
        yield return new WaitForSeconds(
            attackDelay
        );

        // 원래 색상 복구
        if (spriteRenderer != null)
        {
            spriteRenderer.color =
                originalColor;
        }

        // 공격 직전 거리 재확인
        float distance =
            Vector2.Distance(
                transform.position,
                player.position
            );

        // 아직 공격 범위 안에 있으면 공격
        if (distance <= attackRange)
        {
            Attack();
        }

        // 공격 쿨타임 대기
        yield return new WaitForSeconds(
            attackCooldown
        );

        // 공격 종료
        isAttacking = false;
    }

    // =========================
    // 실제 공격 처리
    // =========================
    void Attack()
    {
        PlayerParry playerParry =
            player.GetComponent<PlayerParry>();

        // 플레이어가 패링 중이면
        if (playerParry != null &&
            playerParry.IsParrying())
        {
            Debug.Log("Parried!");

            // 패링 성공 처리
            playerParry.OnSuccessfulParry(
                gameObject
            );

            return;
        }

        // 플레이어 체력 컴포넌트 가져오기
        PlayerHealth playerHealth =
            player.GetComponent<PlayerHealth>();

        // 데미지 적용
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(
                attackDamage
            );

            Debug.Log(
                "Enemy Attack!"
            );
        }
    }

    // =========================
    // 감지 범위 / 공격 범위 표시
    // =========================
    void OnDrawGizmos()
    {
        // 감지 범위 표시
        Gizmos.color =
            Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        // 공격 범위 표시
        Gizmos.color =
            Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }

    // =========================
    // 현재 공격 강제 취소
    // =========================
    public void CancelAttack()
    {
        // 실행 중인 모든 코루틴 정지
        StopAllCoroutines();

        // 공격 상태 해제
        isAttacking = false;

        // 색상 원상복구
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}