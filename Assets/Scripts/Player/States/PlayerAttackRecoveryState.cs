using UnityEngine;

public class PlayerAttackRecoveryState : IState
{
    private readonly PlayerController player;

    private float timer;
    private float duration;
    private float elapsed;

    public PlayerAttackRecoveryState(
        PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        WeaponData weapon =
            player.Weapon.CurrentWeapon;

        duration = Mathf.Max(0.01f, weapon.attackRecovery);
        timer = duration;
        elapsed = 0f;
        player.SetPoise(weapon.attackPoise);
        player.Movement.Stop();
        player.SetAttackVisual("Recovery", 0f);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            player.QueueNextComboAttack();

        elapsed += Time.deltaTime;
        timer -= Time.deltaTime;
        player.SetAttackVisual("Recovery", elapsed / duration);

        if (timer <= 0f)
        {
            if (player.AdvanceQueuedComboAttack())
            {
                player.StateMachine.ChangeState(player.AttackStartupState);
                return;
            }

            player.ResetAttackCombo();
            float moveInput =
                Input.GetAxisRaw("Horizontal");

            if (moveInput == 0)
            {
                player.StateMachine.ChangeState(
                    player.IdleState);
            }
            else
            {
                player.StateMachine.ChangeState(
                    player.MoveState);
            }
        }
    }

    public void Exit()
    {
    }
}
