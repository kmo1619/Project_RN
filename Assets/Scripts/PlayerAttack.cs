using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    public Transform attackPoint;      // 공격 중심점
    public float attackRange = 0.5f;   // 공격 범위
    public int attackDamage = 10;      // 공격 데미지

    public LayerMask enemyLayer;       // 적 레이어

    void Update()
    {
        // 공격 입력
        if (Input.GetKeyDown(KeyCode.D))
        {
            Attack();
        }
    }

    void Attack()
    {
        // 공격 범위 내 적 탐색
        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(
                attackPoint.position,
                attackRange,
                enemyLayer
            );

        // 탐지된 적 처리
        foreach (Collider2D enemy in enemies)
        {
            Debug.Log("Enemy hit: " + enemy.name);

            EnemyHealth enemyHealth =
                enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                // 데미지 적용
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    //void OnDrawGizmosSelected()
    //{
    //    if (attackPoint == null)
    //        return;

    //    Gizmos.color = Color.red;

    //    // 공격 범위 표시
    //    Gizmos.DrawWireSphere(
    //        attackPoint.position,
    //        attackRange
    //    );
    //}
    // 항상 기즈모 표시
    void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            attackPoint.position,
            attackRange
        );
    }
}