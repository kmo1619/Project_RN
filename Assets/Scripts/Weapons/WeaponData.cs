using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public string weaponName;

    [Header("Attack")]
    public int attackDamage;
    public float attackRange;

    [Header("Attack Timing")]
    public float attackStartup;
    public float attackActiveDuration = 0.12f;
    public float attackRecovery;

    [Header("Dash Attack Timing")]
    public float dashAttackStartup;
    public float dashAttackActiveDuration = 0.06f;
    public float dashAttackRecovery;

    [Header("Hit Stun")]
    public int staggerPower;

    [Header("Knockdown")]
    public int knockdownDamage;

    [Header("Poise")]
    public int attackPoise;
    public int parryStartupPoise = 50;
    public int parryActivePoise = 50;
    public int parryRecoveryPoise = 10;

    [Header("Parry")]
    public float parryStartup;
    public float parryDuration;
    public float parryRecovery;
}
