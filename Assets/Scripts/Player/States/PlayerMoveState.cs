using UnityEngine;

public class PlayerMoveState : IState
{
    private readonly PlayerController player;

    public PlayerMoveState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.SetPoise(player.stats.movePoise);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            player.StateMachine.ChangeState(
                player.AttackStartupState);

            return;
        }

        float moveInput =
            Input.GetAxisRaw("Horizontal");

        if (moveInput == 0)
        {
            player.StateMachine.ChangeState(
                player.IdleState);

            return;
        }

        player.Movement.Move(moveInput);
        player.Movement.Flip(moveInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Movement.Jump();
        }
    }

    public void Exit()
    {
    }
}
