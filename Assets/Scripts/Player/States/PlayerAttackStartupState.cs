using UnityEngine;

public class PlayerAttackStartupState : IState
{
    private PlayerController player;

    private float timer;

    public PlayerAttackStartupState(
        PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        timer =
            player.Weapon.CurrentWeapon.attackStartup;
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            player.Combat.PerformAttack();

            player.StateMachine.ChangeState(
                player.AttackRecoveryState);
        }
    }

    public void Exit()
    {
    }
}