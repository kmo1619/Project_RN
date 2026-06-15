using UnityEngine;

public class PlayerAttackRecoveryState : IState
{
    private readonly PlayerController player;

    private float timer;

    public PlayerAttackRecoveryState(
        PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        WeaponData weapon =
            player.Weapon.CurrentWeapon;

        timer = weapon.attackRecovery;
        player.SetPoise(weapon.attackPoise);
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            float moveInput =
                Input.GetAxisRaw("Horizontal");

            if (moveInput == 0)
            {
                player.StateMachine.ChangeState(
                    player.IdleState);
            }
            else
            {
                player.StateMachine.ChangeState(
                    player.MoveState);
            }
        }
    }

    public void Exit()
    {
    }
}
