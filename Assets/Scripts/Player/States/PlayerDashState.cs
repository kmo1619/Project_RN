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
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}
