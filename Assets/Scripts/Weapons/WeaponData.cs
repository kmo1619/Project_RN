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
    public float attackRecovery;

    [Header("Hit Stun")]
    public int staggerPower;

    [Header("Knockdown")]
    public int knockdownDamage;

    [Header("Parry")]
    public float parryStartup;
    public float parryDuration;
}