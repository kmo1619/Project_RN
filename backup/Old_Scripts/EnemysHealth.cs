using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // =========================
    // 체력 설정
    // =========================
    [Header("Health")]
    public int maxHealth = 30; // 최대 체력

    // 현재 체력
    private int currentHealth;

    void Start()
    {
        // =========================
        // 게임 시작 시
        // 현재 체력을 최대 체력으로 초기화
        // =========================
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // =========================
        // 데미지 적용
        // =========================
        currentHealth -= damage;

        // 콘솔에 현재 체력 출력
        Debug.Log(
            gameObject.name +
            " HP : " +
            currentHealth
        );

        // =========================
        // 체력이 0 이하가 되면 사망
        // =========================
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // =========================
        // 사망 로그 출력
        // =========================
        Debug.Log(gameObject.name + " Died");

        // =========================
        // 오브젝트 제거
        // =========================
        Destroy(gameObject);
    }
}