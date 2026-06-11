using UnityEngine;

public class ForgottenOneAttackReadyState : IState
{
    private ForgottenOneController enemy;

    private float timer;

    public ForgottenOneAttackReadyState(
        ForgottenOneController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        timer = enemy.stats.attackStartup;
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            enemy.GetComponent<EnemyCombat>()
     .PerformAttack();

            enemy.StateMachine.ChangeState(
                enemy.AttackRecoveryState);
        }
    }

    public void Exit()
    {
    }
}