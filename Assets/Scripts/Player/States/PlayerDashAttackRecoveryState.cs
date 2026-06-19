using UnityEngine;

public class PlayerDashAttackRecoveryState : IState
{
    private readonly PlayerController player;

    private float timer;

    public PlayerDashAttackRecoveryState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        WeaponData weapon = player.Weapon.CurrentWeapon;

        timer = weapon.attackRecovery;
        player.SetPoise(weapon.attackPoise);
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");

            if (moveInput != 0f)
                player.StateMachine.ChangeState(player.MoveState);
            else
                player.StateMachine.ChangeState(player.IdleState);
        }
    }

    public void Exit()
    {
    }
}
