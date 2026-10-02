using UnityEngine;

public class EnemyHealth :
    MonoBehaviour,
    IDamageable
{
    private const int KnockdownDamageMultiplier = 10;

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
        int appliedDamage = combat != null && combat.IsKnockedDown
            ? damage * KnockdownDamageMultiplier
            : damage;

        int hpBefore = currentHealth;
        currentHealth -= appliedDamage;

#if UNITY_EDITOR
        Debug.Log(
            $"[DAMAGE][ENEMY] {hpBefore}->{currentHealth} ({currentHealth - hpBefore})");
#endif

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            return;
        }

        combat?.ApplyStagger(staggerPower);
    }
}
