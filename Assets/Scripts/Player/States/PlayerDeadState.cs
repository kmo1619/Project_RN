using UnityEngine;

public class PlayerDeadState : IState
{
    private readonly PlayerController player;

    public PlayerDeadState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Movement.Stop();
        player.SetPoise(999999);
#if UNITY_EDITOR
        Debug.Log("[DEAD]");
#endif
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}
