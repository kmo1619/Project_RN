using UnityEngine;

public class PlayerAttackActiveState : IState
{
    private const float ActiveDuration = 0.12f;
    private readonly PlayerController player;
    private float elapsed;

    public PlayerAttackActiveState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        elapsed = 0f;
        player.Combat.PerformAttack();
        player.SetAttackVisual("Attack", 0f);
    }

    public void Update()
    {
        elapsed += Time.deltaTime;
        player.SetAttackVisual("Attack", elapsed / ActiveDuration);

        if (elapsed >= ActiveDuration)
            player.StateMachine.ChangeState(player.AttackRecoveryState);
    }

    public void Exit() { }
}
