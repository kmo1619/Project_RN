using UnityEngine;

public class PlayerDashState : IState
{
    private readonly PlayerController player;

    public PlayerDashState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.SetPoise(player.stats.dashPoise);
        player.Movement.StartDash(new Vector2(player.Dash.DashDirection, 0f));
        player.Movement.Flip(player.Dash.DashDirection);
        player.Health.SetInvincible(true);
    }

    public void Update()
    {
        // 우선순위 1: DashAttack 입력
        if (Input.GetKeyDown(KeyCode.D))
        {
            player.StateMachine.ChangeState(player.DashAttackStartupState);
            return;
        }

        // 우선순위 2: 대시 방향 벽 충돌
        if ((player.Dash.DashDirection > 0f && player.Movement.IsWallRight) ||
            (player.Dash.DashDirection < 0f && player.Movement.IsWallLeft))
        {
            EndDash();
            return;
        }

        // 우선순위 3: 목표 거리 도달
        float traveled = Vector2.Distance(
            player.Movement.DashStartPosition,
            (Vector2)player.Movement.transform.position);

        if (traveled >= player.stats.dashDistance)
        {
            EndDash();
            return;
        }
    }

    public void Exit()
    {
        player.Movement.StopDash();
        player.Health.SetInvincible(false);
    }

    private void EndDash()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput != 0f)
            player.StateMachine.ChangeState(player.MoveState);
        else
            player.StateMachine.ChangeState(player.IdleState);
    }
}
