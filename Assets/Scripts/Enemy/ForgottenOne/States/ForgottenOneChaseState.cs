using UnityEngine;

public class ForgottenOneChaseState : IState
{
    private ForgottenOneController enemy;

    public ForgottenOneChaseState(
        ForgottenOneController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
    }

    public void Update()
    {
        Transform player =
            enemy.GetPlayer();

        if (player == null)
            return;

        float distance =
            enemy.DistanceToPlayer();

        if (distance <= enemy.stats.attackRange)
        {
            enemy.StateMachine.ChangeState(
                enemy.AttackReadyState);

            return;
        }

        Vector2 direction =
            new Vector2(
                player.position.x -
                enemy.transform.position.x,
                0f).normalized;

        enemy.transform.position +=
            (Vector3)direction *
            enemy.stats.moveSpeed *
            Time.deltaTime;
    }

    public void Exit()
    {
    }
}