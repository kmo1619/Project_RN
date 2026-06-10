using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 30; // 최대 체력

    private int currentHealth; // 현재 체력

    void Start()
    {
        // 게임 시작 시 최대 체력 설정
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        // 데미지 적용
        currentHealth -= damage;

        Debug.Log(
            gameObject.name +
            " HP : " +
            currentHealth
        );

        // 체력이 0 이하가 되면 사망
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " Died");

        // 적 제거
        Destroy(gameObject);
    }
}