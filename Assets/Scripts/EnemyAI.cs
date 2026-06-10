using UnityEngine;
using System.Collections;
public class EnemyAI : MonoBehaviour
{
    [Header("Target")]
    public Transform player; // 추적할 플레이어

    [Header("Attack")]
    public float attackRange = 1.2f;      // 공격 가능 거리
    public int attackDamage = 10;       // 공격 데미지
    public float attackCooldown = 1f;   // 공격 쿨타임(초)

    [Header("Movement")]
    public float moveSpeed = 2f;        // 이동 속도

    [Header("Detection")]
    public float detectionRange = 5f;   // 플레이어 감지 범위

    private float lastAttackTime;       // 마지막 공격 시간

    [Header("Attack Timing")]
    public float attackDelay = 0.7f; // 전조 시간

    private bool isAttacking; // 공격 준비 중 여부

    private SpriteRenderer spriteRenderer; // 적 스프라이트

    private Color originalColor; // 원래 색상


    void Start()
    {
        // SpriteRenderer 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 원래 색상 저장
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        // 공격 준비 중이면 아무 행동도 하지 않음
        if (isAttacking)
        {
            return;
        }

        // 플레이어가 없으면 종료
        if (player == null)
            return;

        // 플레이어와 적 사이 거리 계산
        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        // 공격 범위 안이면 공격
        if (distance <= attackRange)
        {
            if (!isAttacking)
            {
                StartCoroutine(AttackRoutine());
            }
        }
        // 감지 범위 안이면 플레이어 추적
        else if (distance <= detectionRange)
        {
            FollowPlayer();
        }
        // 플레이어 방향 바라보기
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void FollowPlayer()
    {
        // X축만 따라가도록 목표 위치 설정
        Vector3 targetPosition = new Vector3(
            player.position.x,
            transform.position.y,
            transform.position.z
        );

        // 목표 위치로 이동
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    void Attack()
    {
        PlayerParry playerParry =
            player.GetComponent<PlayerParry>();

        // 패링 성공
        if (playerParry != null &&
            playerParry.IsParrying())
        {
            Debug.Log("Parried!");

            return;
        }

        PlayerHealth playerHealth =
            player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);

            Debug.Log("Enemy Attack!");
        }
    }

    void OnDrawGizmos()
    {
        // 감지 범위 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        // 공격 범위 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );
    }


    //전조 후 공격
    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        Debug.Log("Attack Ready!");

        spriteRenderer.color = Color.yellow;

        yield return new WaitForSeconds(attackDelay);

        spriteRenderer.color = originalColor;

        // 공격 직전에 거리 재확인
        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance <= attackRange)
        {
            Attack();
        }

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }
}