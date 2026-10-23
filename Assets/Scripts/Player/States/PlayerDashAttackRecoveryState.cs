using UnityEngine;

public class PlayerDashAttackRecoveryState : IState
{
    private readonly PlayerController player;

    private float timer;
    private float duration;
    private float elapsed;

    public PlayerDashAttackRecoveryState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        WeaponData weapon = player.Weapon.CurrentWeapon;

        duration = Mathf.Max(0.01f, weapon.dashAttackRecovery);
        timer = duration;
        elapsed = 0f;
        player.SetPoise(weapon.attackPoise);
        player.SetDashAttackVisual("Recovery", 0f);
    }

    public void Update()
    {
        elapsed += Time.deltaTime;
        timer -= Time.deltaTime;
        player.SetDashAttackVisual("Recovery", elapsed / duration);

        if (timer <= 0f)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");

            if (moveInput != 0f)
                player.StateMachine.ChangeState(player.MoveState);
            else
                player.StateMachine.ChangeState(player.IdleState);
        }
    }

    public void Exit()
    {
    }
}
