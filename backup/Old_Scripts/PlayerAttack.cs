using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    public Transform attackPoint;
    public LayerMask enemyLayer;

    private PlayerWeapon playerWeapon;
    private PlayerStateMachine stateMachine;

    void Start()
    {
        playerWeapon =
            GetComponent<PlayerWeapon>();

        stateMachine =
            GetComponent<PlayerStateMachine>();
    }

    public void StartAttack()
    {
        if (
            !stateMachine.IsState(PlayerState.Idle) &&
            !stateMachine.IsState(PlayerState.Move)
        )
        {
            return;
        }

        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        WeaponData weapon =
            playerWeapon.currentWeapon;

        stateMachine.ChangeState(
            PlayerState.AttackStartup
        );

        Debug.Log("Attack Startup");

        yield return new WaitForSeconds(
            weapon.attackStartup
        );

        Attack();

        stateMachine.ChangeState(
            PlayerState.AttackRecovery
        );

        Debug.Log("Attack Recovery");

        yield return new WaitForSeconds(
            weapon.attackRecovery
        );

        if (
            Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow)
        )
        {
            stateMachine.ChangeState(
                PlayerState.Move
            );
        }
        else
        {
            stateMachine.ChangeState(
                PlayerState.Idle
            );
        }

        Debug.Log("Attack End");
    }

    void Attack()
    {
        Debug.Log("Actual Attack");

        WeaponData weapon =
            playerWeapon.currentWeapon;

        Collider2D[] enemies =
            Physics2D.OverlapCircleAll(
                attackPoint.position,
                weapon.attackRange,
                enemyLayer
            );

        foreach (Collider2D enemy in enemies)
        {
            Debug.Log(
                "Enemy Hit : " +
                enemy.name
            );

            EnemyHealth enemyHealth =
                enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(
                    weapon.attackDamage
                );
            }

            EnemyCombat enemyCombat =
                enemy.GetComponent<EnemyCombat>();

            if (enemyCombat != null)
            {
                enemyCombat.TryHitStun(
                    weapon.staggerPower
                );
            }
        }
    }

    void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;

        float range = 1f;

        if (Application.isPlaying)
        {
            PlayerWeapon weapon =
                GetComponent<PlayerWeapon>();

            if (
                weapon != null &&
                weapon.currentWeapon != null
            )
            {
                range =
                    weapon.currentWeapon.attackRange;
            }
        }

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            attackPoint.position,
            range
        );
    }
}