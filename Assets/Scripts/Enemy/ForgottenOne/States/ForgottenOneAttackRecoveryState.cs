using UnityEngine;

public class ForgottenOneAttackRecoveryState : IState
{
    private ForgottenOneController enemy;

    private float timer;

    public ForgottenOneAttackRecoveryState(
        ForgottenOneController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        timer = enemy.stats.attackRecovery;
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            enemy.StateMachine.ChangeState(
                enemy.ChaseState);
        }
    }

    public void Exit()
    {
    }
}
