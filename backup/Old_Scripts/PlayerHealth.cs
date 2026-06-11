using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // =========================
    // 체력 설정
    // =========================
    [Header("Health")]
    public int maxHealth = 100; // 최대 체력

    // 현재 체력
    private int currentHealth;

    void Start()
    {
        // 게임 시작 시 현재 체력을 최대 체력으로 설정
        currentHealth = maxHealth;
    }

    // =========================
    // 플레이어 피격 처리
    // =========================
    public void TakeDamage(int damage)
    {
        // 데미지 적용
        currentHealth -= damage;

        // 현재 체력 출력
        Debug.Log(
            "Player HP : " +
            currentHealth
        );

        // 체력이 0 이하가 되면 사망
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // =========================
    // 플레이어 사망 처리
    // =========================
    void Die()
    {
        Debug.Log("Player Died");

        // 임시 사망 처리
        // 플레이어 오브젝트 삭제
        Destroy(gameObject);
    }
}