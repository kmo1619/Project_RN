using UnityEngine;

public class EnemyHitStunState : IState
{
    private readonly EnemyController enemy;
    private readonly EnemyVisual visual;

    private float timer;

    public EnemyHitStunState(EnemyController enemy)
    {
        this.enemy = enemy;
        visual = enemy.GetComponent<EnemyVisual>();
    }

    public void Enter()
    {
        timer = enemy.stats.hitStunDuration;
        visual?.ShowHitStun();
#if UNITY_EDITOR
        Debug.Log("[HITSTUN][ENEMY]");
#endif
    }

    public void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            enemy.StateMachine.ChangeState(
                enemy.GetStateAfterHitStun());
        }
    }

    public void Exit()
    {
        visual?.ClearHitStun();
    }
}
