using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerController player;

    public PlayerIdleState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Movement.Stop();
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

        if (moveInput != 0)
        {
            player.StateMachine.ChangeState(
                player.MoveState);

            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Movement.Jump();
        }
    }

    public void Exit()
    {
    }
}