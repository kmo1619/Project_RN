using UnityEngine;

public class EnemyIdleState : IState
{
    protected EnemyController enemy;

    public EnemyIdleState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public virtual void Enter()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Exit()
    {
    }
}
