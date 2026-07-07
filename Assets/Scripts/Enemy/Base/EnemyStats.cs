using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Type")]
    public EnemySize size = EnemySize.Medium;

    [Header("Health")]
    public int maxHealth = 100;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Detection")]
    public float detectRange = 8f;
    public float chaseRange = 10f;

    [Header("Attack")]
    public int attackDamage = 10;

    public float attackRange = 1.5f;

    public float attackStartup = 0.5f;

    public float attackRecovery = 0.5f;

    public int attackStaggerPower = 15;

    [Header("Poise")]
    public int poise = 10;

    [Header("Knockdown")]
    public int maxKnockdownGauge = 100;

    public float knockdownDuration = 2f;

    public int parryKnockdownPower = 30;

    [Header("HitStun")]
    public float hitStunDuration = 0.3f;
}
