using UnityEngine;

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

    void Update()
    {
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
            Attack();
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
        // 공격 쿨타임 확인
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        // 마지막 공격 시간 저장
        lastAttackTime = Time.time;

        // 플레이어 체력 스크립트 가져오기
        PlayerHealth playerHealth =
            player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            // 플레이어에게 데미지 적용
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
}