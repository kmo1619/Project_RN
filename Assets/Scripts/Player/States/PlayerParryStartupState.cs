using UnityEngine;

public class PlayerParryStartupState : IState
{
    private readonly PlayerController player;

    public PlayerParryStartupState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.SetPoise(player.stats.parryStartupPoise);
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}
