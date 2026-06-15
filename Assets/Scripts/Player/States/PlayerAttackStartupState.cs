using UnityEngine;

public class PlayerAttackStartupState : IState
{
    private readonly PlayerController player;

    private float timer;

    public PlayerAttackStartupState(
        PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        WeaponData weapon =
            player.Weapon.CurrentWeapon;

        timer = weapon.attackStartup;
        player.SetPoise(weapon.attackPoise);
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
