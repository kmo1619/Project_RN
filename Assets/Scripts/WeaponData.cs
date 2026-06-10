using UnityEngine;

[System.Serializable]
public class WeaponData
{
    [Header("Info")]
    public string weaponName;

    [Header("Attack")]
    public int attackDamage;
    public float attackRange;

    [Header("Parry")]
    public float parryStartup;
    public float parryDuration;

    [Header("Stagger")]
    public int staggerDamage;
}