using UnityEngine;

public abstract class EnemyController : MonoBehaviour, IStateMachineProvider
{
    public EnemyStats stats;

    protected Transform player;

    public StateMachine StateMachine { get; protected set; }

    public EnemyHitStunState HitStunState { get; protected set; }

    protected IState stateBeforeHitStun;

    protected virtual void Awake()
    {
        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        StateMachine = new StateMachine();

        EnsureVisualComponent();
        HitStunState = new EnemyHitStunState(this);
    }

    protected virtual void Update()
    {
        StateMachine?.Update();
    }

    public void EnterHitStun()
    {
        if (StateMachine.CurrentState != HitStunState)
        {
            stateBeforeHitStun = StateMachine.CurrentState;
        }

        StateMachine.ChangeState(HitStunState);
    }

    public IState GetStateAfterHitStun()
    {
        return stateBeforeHitStun;
    }

    public float DistanceToPlayer()
    {
        if (player == null)
            return Mathf.Infinity;

        return Vector2.Distance(
            transform.position,
            player.position);
    }

    public Transform GetPlayer()
    {
        return player;
    }

    private void EnsureVisualComponent()
    {
        if (GetComponent<EnemyVisual>() != null)
            return;

        if (GetComponent<SpriteRenderer>() == null)
            return;

        gameObject.AddComponent<EnemyVisual>();
    }
}
