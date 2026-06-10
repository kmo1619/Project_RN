using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100; // 최대 체력

    private int currentHealth;  // 현재 체력

    void Start()
    {
        // 시작 시 최대 체력 설정
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // 데미지 적용
        currentHealth -= damage;

        Debug.Log(
            "Player HP : " +
            currentHealth
        );

        // 체력이 0 이하이면 사망
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died");

        // 임시 처리
        Destroy(gameObject);
    }
}