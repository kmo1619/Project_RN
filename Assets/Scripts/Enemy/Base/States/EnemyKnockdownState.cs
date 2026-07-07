using UnityEngine;

public class EnemyKnockdownState : IState
{
    private readonly EnemyController enemy;
    private readonly EnemyVisual visual;

    private float timer;

    public EnemyKnockdownState(EnemyController enemy)
    {
        this.enemy = enemy;
        visual = enemy.GetComponent<EnemyVisual>();
    }

    public void Enter()
    {
        timer = enemy.stats.knockdownDuration;
        visual?.ShowKnockdown();

#if UNITY_EDITOR
        Debug.Log("[KNOCKDOWN][ENEMY]");
#endif
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            enemy.StateMachine.ChangeState(
                enemy.GetStateAfterKnockdown());
        }
    }

    public void Exit()
    {
        visual?.ClearKnockdown();
    }
}
