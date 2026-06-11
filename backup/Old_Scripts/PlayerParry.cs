using UnityEngine;
using System.Collections;

public class PlayerParry : MonoBehaviour
{
    private PlayerWeapon playerWeapon;
    private PlayerStateMachine stateMachine;

    void Start()
    {
        playerWeapon =
            GetComponent<PlayerWeapon>();

        stateMachine =
            GetComponent<PlayerStateMachine>();
    }

    public bool IsParrying()
    {
        return stateMachine.IsState(
            PlayerState.ParryActive
        );
    }

    public void StartParry()
    {
        if (
            !stateMachine.IsState(PlayerState.Idle) &&
            !stateMachine.IsState(PlayerState.Move)
        )
        {
            return;
        }

        StartCoroutine(
            ParryRoutine()
        );
    }

    IEnumerator ParryRoutine()
    {
        stateMachine.ChangeState(
            PlayerState.ParryStartup
        );

        Debug.Log(
            "Parry Prepare"
        );

        yield return new WaitForSeconds(
            playerWeapon.currentWeapon.parryStartup
        );

        stateMachine.ChangeState(
            PlayerState.ParryActive
        );

        Debug.Log(
            "Parry Start"
        );

        yield return new WaitForSeconds(
            playerWeapon.currentWeapon.parryDuration
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

        Debug.Log(
            "Parry End"
        );
    }

    public void OnSuccessfulParry(
        GameObject enemy
    )
    {
        EnemyCombat enemyCombat =
            enemy.GetComponent<EnemyCombat>();

        if (enemyCombat != null)
        {
            enemyCombat.AddKnockdownGauge(
                playerWeapon.currentWeapon.knockdownDamage
            );
        }

        ConsumeParry();
    }

    public void ConsumeParry()
    {
        StopAllCoroutines();

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

        Debug.Log(
            "Parry Consumed"
        );
    }
}