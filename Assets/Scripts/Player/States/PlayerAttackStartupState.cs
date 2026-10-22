using UnityEngine;

public class PlayerAttackStartupState : IState
{
    private readonly PlayerController player;

    private float timer;
    private float duration;
    private float elapsed;

    public PlayerAttackStartupState(
        PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        WeaponData weapon =
            player.Weapon.CurrentWeapon;

        duration = Mathf.Max(0.01f, weapon.attackStartup);
        timer = duration;
        elapsed = 0f;
        player.SetPoise(weapon.attackPoise);
        player.Movement.Decelerate(
            player.stats.attackStartupDeceleration);
        player.SetAttackVisual("Startup", 0f);
    }

    public void Update()
    {
        elapsed += Time.deltaTime;
        timer -= Time.deltaTime;
        player.SetAttackVisual("Startup", elapsed / duration);

        if (timer <= 0f)
        {
            player.StateMachine.ChangeState(player.AttackActiveState);
        }
    }

    public void Exit()
    {
    }
}
