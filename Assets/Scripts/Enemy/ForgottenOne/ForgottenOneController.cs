using UnityEngine;

public class ForgottenOneController : EnemyController
{
    public ForgottenOneChaseState ChaseState { get; private set; }

    public ForgottenOneAttackReadyState AttackReadyState { get; private set; }

    public ForgottenOneAttackRecoveryState AttackRecoveryState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        ChaseState          = new ForgottenOneChaseState(this);
        AttackReadyState    = new ForgottenOneAttackReadyState(this);
        AttackRecoveryState = new ForgottenOneAttackRecoveryState(this);

        knockdownRecoveryState = ChaseState;
    }

    private void Start()
    {
        StateMachine.Initialize(ChaseState);
    }
}
