using UnityEngine;

public class PlayerHitStunState : IState
{
    private readonly PlayerController player;

    private float timer;

    public PlayerHitStunState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Movement.Stop();
        timer = player.stats.hitStunDuration;
#if UNITY_EDITOR
        Debug.Log("[HITSTUN]");
#endif
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            player.StateMachine.ChangeState(
                player.IdleState);
        }
    }

    public void Exit()
    {
    }
}
