using UnityEngine;

public class PlayerIdleState : IState
{
    private readonly PlayerController player;

    public PlayerIdleState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Movement.Stop();
        player.SetPoise(player.stats.idlePoise);
    }

    public void Update()
    {
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
