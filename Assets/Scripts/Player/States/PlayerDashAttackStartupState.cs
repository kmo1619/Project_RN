using UnityEngine;

public class PlayerDashAttackStartupState : IState
{
    private readonly PlayerController player;

    private float timer;

    public PlayerDashAttackStartupState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        WeaponData weapon = player.Weapon.CurrentWeapon;

        timer = weapon.attackStartup;
        player.SetPoise(weapon.attackPoise);
        player.Movement.Stop();
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            player.Combat.PerformAttack();

            player.StateMachine.ChangeState(
                player.DashAttackRecoveryState);
        }
    }

    public void Exit()
    {
    }
}
