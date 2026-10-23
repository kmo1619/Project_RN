using UnityEngine;

public class PlayerAttackActiveState : IState
{
    private readonly PlayerController player;
    private float elapsed;
    private float activeDuration;

    public PlayerAttackActiveState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        elapsed = 0f;
        activeDuration = Mathf.Max(0.01f, player.Weapon.CurrentWeapon.attackActiveDuration);
        player.Combat.PerformAttack();
        player.SetAttackVisual("Attack", 0f, activeDuration);
    }

    public void Update()
    {
        elapsed += Time.deltaTime;
        player.SetAttackVisual("Attack", elapsed / activeDuration, activeDuration);

        if (elapsed >= activeDuration)
            player.StateMachine.ChangeState(player.AttackRecoveryState);
    }

    public void Exit() { }
}
