using UnityEngine;

public class EnemyHealth :
    MonoBehaviour,
    IDamageable
{
    public int maxHealth = 100;

    private int currentHealth;

    private EnemyCombat combat;

    private void Awake()
    {
        combat = GetComponent<EnemyCombat>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(
        int damage,
        int staggerPower)
    {
        currentHealth -= damage;

        Debug.Log(
            $"Enemy HP : {currentHealth}");

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            return;
        }

        combat?.ApplyStagger(staggerPower);
    }
}
