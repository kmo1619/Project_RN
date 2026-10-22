using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData straightSword;

    public WeaponData dagger;

    public WeaponData greatSword;

    public WeaponData CurrentWeapon { get; private set; }

    public WeaponType CurrentWeaponType { get; private set; } = WeaponType.StraightSword;

    private void Start()
    {
        EquipWeapon(WeaponType.StraightSword);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(WeaponType.StraightSword);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(WeaponType.Dagger);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(WeaponType.GreatSword);
        }
    }

    public void EquipWeapon(WeaponType type)
    {
        CurrentWeaponType = type;

        switch (type)
        {
            case WeaponType.StraightSword:
                CurrentWeapon = straightSword;
                break;

            case WeaponType.Dagger:
                CurrentWeapon = dagger;
                break;

            case WeaponType.GreatSword:
                CurrentWeapon = greatSword;
                break;
        }

        Debug.Log($"Equip : {CurrentWeapon.weaponName}");
    }
}
