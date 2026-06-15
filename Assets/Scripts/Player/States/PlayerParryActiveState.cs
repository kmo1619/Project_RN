using UnityEngine;

public class PlayerParryActiveState : IState
{
    private readonly PlayerController player;

    public PlayerParryActiveState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.SetPoise(player.stats.parryActivePoise);
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}
