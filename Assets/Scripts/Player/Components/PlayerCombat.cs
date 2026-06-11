using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerController controller;

    [Header("Attack")]
    public Transform attackPoint;

    public LayerMask enemyLayer;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    public void PerformAttack()
    {
        WeaponData weapon = controller.Weapon.CurrentWeapon;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                attackPoint.position,
                weapon.attackRange,
                enemyLayer);

        foreach (Collider2D hit in hits)
        {
            IDamageable damageable =
                hit.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(
                    weapon.attackDamage,
                    weapon.staggerPower);
            }
        }

        Debug.Log(
            $"Attack : {weapon.weaponName}");
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            attackPoint.position,
            1f);
    }
#endif
}