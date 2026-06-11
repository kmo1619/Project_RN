using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    // =========================
    // 장착 가능한 무기 목록
    // =========================
    [Header("Weapons")]
    public WeaponData straightSword; // 직검 데이터
    public WeaponData dagger;        // 단검 데이터
    public WeaponData greatSword;    // 대검 데이터

    // =========================
    // 현재 장착 무기
    // =========================
    [Header("Current Weapon")]
    public WeaponData currentWeapon;

    private void Start()
    {
        // 게임 시작 시 직검 장착
        EquipWeapon(WeaponType.StraightSword);
    }

    private void Update()
    {
        // =========================
        // 테스트용 무기 변경
        // 1 : 직검
        // 2 : 단검
        // 3 : 대검
        // =========================

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

    // =========================
    // 무기 장착
    // =========================
    public void EquipWeapon(WeaponType weaponType)
    {
        switch (weaponType)
        {
            // 직검 장착
            case WeaponType.StraightSword:
                currentWeapon = straightSword;
                break;

            // 단검 장착
            case WeaponType.Dagger:
                currentWeapon = dagger;
                break;

            // 대검 장착
            case WeaponType.GreatSword:
                currentWeapon = greatSword;
                break;
        }

        // 현재 장착 무기 출력
        Debug.Log(
            "Weapon Changed : " +
            currentWeapon.weaponName
        );
    }
}