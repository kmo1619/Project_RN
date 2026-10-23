using UnityEngine;

public class PlayerDashAttackActiveState : IState
{
    private readonly PlayerController player;
    private float duration;
    private float elapsed;

    public PlayerDashAttackActiveState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        duration = Mathf.Max(0.01f, player.Weapon.CurrentWeapon.dashAttackActiveDuration);
        elapsed = 0f;
        player.Combat.PerformAttack();
        player.SetDashAttackVisual("Attack", 0f);
    }

    public void Update()
    {
        elapsed += Time.deltaTime;
        player.SetDashAttackVisual("Attack", elapsed / duration);
        if (elapsed >= duration)
            player.StateMachine.ChangeState(player.DashAttackRecoveryState);
    }

    public void Exit() { }
}
