using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    public EnemyStats stats;

    protected Transform player;

    public StateMachine StateMachine { get; protected set; }

    protected virtual void Awake()
    {
        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        StateMachine = new StateMachine();
    }

    protected virtual void Update()
    {
        StateMachine?.Update();
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
}