using UnityEngine;

public class PlayerParryRecoveryState : IState
{
    private readonly PlayerController player;

    private float timer;

    public PlayerParryRecoveryState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        timer = player.Weapon.CurrentWeapon.parryRecovery;
        player.Parry.ResetParryResult();
        player.SetPoise(player.Weapon.CurrentWeapon.parryRecoveryPoise);
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
