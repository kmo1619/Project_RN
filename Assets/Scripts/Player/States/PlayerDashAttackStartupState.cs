using UnityEngine;

public class PlayerDashAttackStartupState : IState
{
    private readonly PlayerController player;

    private float timer;
    private float duration;
    private float elapsed;

    public PlayerDashAttackStartupState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        WeaponData weapon = player.Weapon.CurrentWeapon;

        duration = Mathf.Max(0.01f, weapon.dashAttackStartup);
        timer = duration;
        elapsed = 0f;
        player.SetPoise(weapon.attackPoise);
        player.Movement.Stop();
        player.SetDashAttackVisual("Startup", 0f);
    }

    public void Update()
    {
        elapsed += Time.deltaTime;
        timer -= Time.deltaTime;
        player.SetDashAttackVisual("Startup", elapsed / duration);

        if (timer <= 0f)
        {
            player.StateMachine.ChangeState(
                player.DashAttackActiveState);
        }
    }

    public void Exit()
    {
    }
}
