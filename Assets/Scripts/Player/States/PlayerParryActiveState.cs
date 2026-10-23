using UnityEngine;

public class PlayerParryActiveState : IState
{
    private readonly PlayerController player;

    private float timer;

    public PlayerParryActiveState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        timer = player.Weapon.CurrentWeapon.parryDuration;
        player.SetPoise(player.Weapon.CurrentWeapon.parryActivePoise);
        player.Parry.SetWindowActive(true);
    }

    public void Update()
    {
        if (player.Parry.WasParrySuccessful)
        {
            player.StateMachine.ChangeState(player.ParryRecoveryState);
            return;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            player.StateMachine.ChangeState(player.ParryRecoveryState);
        }
    }

    public void Exit()
    {
        player.Parry.SetWindowActive(false);
    }
}
