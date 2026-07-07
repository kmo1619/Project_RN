using UnityEngine;

public class PlayerParryStartupState : IState
{
    private readonly PlayerController player;

    private float timer;

    public PlayerParryStartupState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        timer = player.stats.parryStartupDuration;
        player.Movement.Stop();
        player.SetPoise(player.stats.parryStartupPoise);
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            player.StateMachine.ChangeState(player.ParryActiveState);
        }
    }

    public void Exit()
    {
    }
}
