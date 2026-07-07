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
        if (Input.GetKeyDown(KeyCode.F))
        {
            player.StateMachine.ChangeState(player.ParryStartupState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            player.StateMachine.ChangeState(
                player.AttackStartupState);

            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) &&
            player.Dash.CanDash &&
            player.Movement.IsGrounded)
        {
            float h   = Input.GetAxisRaw("Horizontal");
            float dir = h != 0f ? Mathf.Sign(h) : player.Movement.FacingDirection;

            player.Dash.Prepare(dir);
            player.Dash.UseDash(player.stats.dashCooldown);
            player.StateMachine.ChangeState(player.DashState);
            return;
        }

        float moveInput = Input.GetAxisRaw("Horizontal");

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
