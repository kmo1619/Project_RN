using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public StateMachine StateMachine { get; private set; }

    [Header("Data")]
    public PlayerStats stats;

    [Header("Components")]
    public PlayerMovement Movement { get; private set; }

    public PlayerCombat Combat { get; private set; }

    public PlayerParry Parry { get; private set; }

    public PlayerDash Dash { get; private set; }

    public PlayerHealth Health { get; private set; }

    public PlayerWeapon Weapon { get; private set; }

    public PlayerIdleState IdleState { get; private set; }

    public PlayerMoveState MoveState { get; private set; }

    public PlayerAttackStartupState AttackStartupState { get; private set; }

    public PlayerAttackRecoveryState AttackRecoveryState { get; private set; }

    public PlayerParryStartupState ParryStartupState { get; private set; }

    public PlayerParryActiveState ParryActiveState { get; private set; }

    public PlayerDashState DashState { get; private set; }

    public PlayerDeadState DeadState { get; private set; }

    private void Awake()
    {
        Movement = GetComponent<PlayerMovement>();
        Combat = GetComponent<PlayerCombat>();
        Parry = GetComponent<PlayerParry>();
        Dash = GetComponent<PlayerDash>();
        Health = GetComponent<PlayerHealth>();
        Weapon = GetComponent<PlayerWeapon>();

        StateMachine = new StateMachine();

        IdleState = new PlayerIdleState(this);
        MoveState = new PlayerMoveState(this);

        AttackStartupState = new PlayerAttackStartupState(this);
        AttackRecoveryState = new PlayerAttackRecoveryState(this);

        ParryStartupState = new PlayerParryStartupState(this);
        ParryActiveState = new PlayerParryActiveState(this);

        DashState = new PlayerDashState(this);

        DeadState = new PlayerDeadState(this);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.Update();
    }
}