using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapons")]
    public WeaponData straightSword;
    public WeaponData dagger;
    public WeaponData greatSword;

    [Header("Current")]
    public WeaponData currentWeapon;

    void Start()
    {
        EquipWeapon(WeaponType.StraightSword);
    }

    void Update()
    {
        // 직검
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(WeaponType.StraightSword);
        }

        // 단검
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(WeaponType.Dagger);
        }

        // 대검
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(WeaponType.GreatSword);
        }
    }

    public void EquipWeapon(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.StraightSword:
                currentWeapon = straightSword;
                break;

            case WeaponType.Dagger:
                currentWeapon = dagger;
                break;

            case WeaponType.GreatSword:
                currentWeapon = greatSword;
                break;
        }

        Debug.Log("Weapon Changed : " + currentWeapon.weaponName);
    }
}