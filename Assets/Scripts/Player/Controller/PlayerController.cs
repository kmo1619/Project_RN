using UnityEngine;

public class PlayerController : MonoBehaviour, IStateMachineProvider
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

    private PlayerVisual visual;
    private PlayerWeaponFollower weaponFollower;

    public int currentPoise { get; private set; }

    public PlayerIdleState IdleState { get; private set; }

    public PlayerMoveState MoveState { get; private set; }

    public PlayerAttackStartupState AttackStartupState { get; private set; }

    public PlayerAttackRecoveryState AttackRecoveryState { get; private set; }

    public PlayerAttackActiveState AttackActiveState { get; private set; }

    public int AttackComboIndex { get; private set; }

    private bool queuedComboAttack;

    public PlayerParryStartupState  ParryStartupState  { get; private set; }

    public PlayerParryActiveState   ParryActiveState   { get; private set; }

    public PlayerParryRecoveryState ParryRecoveryState { get; private set; }

    public PlayerDashState DashState { get; private set; }

    public PlayerDashAttackStartupState DashAttackStartupState { get; private set; }

    public PlayerDashAttackActiveState DashAttackActiveState { get; private set; }

    public PlayerDashAttackRecoveryState DashAttackRecoveryState { get; private set; }

    public PlayerHitStunState HitStunState { get; private set; }

    public PlayerDeadState DeadState { get; private set; }

    private void Awake()
    {
        Movement = GetComponent<PlayerMovement>();
        Combat   = GetComponent<PlayerCombat>();
        Parry    = GetComponent<PlayerParry>();
        Dash     = GetComponent<PlayerDash>();
        Health   = GetComponent<PlayerHealth>();
        Weapon   = GetComponent<PlayerWeapon>();
        visual   = GetComponent<PlayerVisual>();
        if (visual == null)
            visual = gameObject.AddComponent<PlayerVisual>();

        weaponFollower = GetComponent<PlayerWeaponFollower>();
        if (weaponFollower == null)
            weaponFollower = gameObject.AddComponent<PlayerWeaponFollower>();

        StateMachine = new StateMachine();

        IdleState = new PlayerIdleState(this);
        MoveState = new PlayerMoveState(this);

        AttackStartupState  = new PlayerAttackStartupState(this);
        AttackActiveState  = new PlayerAttackActiveState(this);
        AttackRecoveryState = new PlayerAttackRecoveryState(this);

        ParryStartupState  = new PlayerParryStartupState(this);
        ParryActiveState   = new PlayerParryActiveState(this);
        ParryRecoveryState = new PlayerParryRecoveryState(this);

        DashState = new PlayerDashState(this);

        DashAttackStartupState  = new PlayerDashAttackStartupState(this);
        DashAttackActiveState = new PlayerDashAttackActiveState(this);
        DashAttackRecoveryState = new PlayerDashAttackRecoveryState(this);

        HitStunState = new PlayerHitStunState(this);

        DeadState = new PlayerDeadState(this);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.Update();
        visual.SetWeapon(Weapon.CurrentWeaponType);
        weaponFollower.SetWeapon(Weapon.CurrentWeaponType);
        visual.SetState(StateMachine.CurrentState);
        if (StateMachine.CurrentState is not PlayerAttackStartupState &&
            StateMachine.CurrentState is not PlayerAttackActiveState &&
            StateMachine.CurrentState is not PlayerAttackRecoveryState &&
            StateMachine.CurrentState is not PlayerDashAttackStartupState &&
            StateMachine.CurrentState is not PlayerDashAttackActiveState &&
            StateMachine.CurrentState is not PlayerDashAttackRecoveryState)
        {
            weaponFollower.ClearGreatSwordAttack();
        }
        visual.Tick();
    }

    public void SetPoise(int value)
    {
        currentPoise = value;
    }

    public void EnterHitStun()
    {
        StateMachine.ChangeState(HitStunState);
    }

    public void BeginAttackCombo()
    {
        AttackComboIndex = 0;
        queuedComboAttack = false;
    }

    public void QueueNextComboAttack()
    {
        if (GetComboHitCount() > 0)
            queuedComboAttack = true;
    }

    public bool AdvanceQueuedComboAttack()
    {
        if (!queuedComboAttack)
            return false;

        queuedComboAttack = false;

        AttackComboIndex = (AttackComboIndex + 1) % GetComboHitCount();
        return true;
    }

    public void ResetAttackCombo()
    {
        AttackComboIndex = 0;
        queuedComboAttack = false;
    }

    public void SetAttackVisual(string phase, float normalizedTime)
    {
        WeaponData weapon = Weapon.CurrentWeapon;
        SetAttackVisual(phase, normalizedTime, weapon.attackStartup, weapon.attackActiveDuration, weapon.attackRecovery, false);
    }

    public void SetAttackVisual(string phase, float normalizedTime, float activeDuration)
    {
        WeaponData weapon = Weapon.CurrentWeapon;
        SetAttackVisual(phase, normalizedTime, weapon.attackStartup, activeDuration, weapon.attackRecovery, false);
    }

    public void SetDashAttackVisual(string phase, float normalizedTime)
    {
        WeaponData weapon = Weapon.CurrentWeapon;
        SetAttackVisual(phase, normalizedTime, weapon.dashAttackStartup, weapon.dashAttackActiveDuration, weapon.dashAttackRecovery, true);
    }

    private void SetAttackVisual(string phase, float normalizedTime, float startup, float activeDuration, float recovery, bool dashAttack)
    {
        if (visual != null)
            visual.SetAttackFrame(Weapon.CurrentWeaponType, AttackComboIndex, phase, normalizedTime);

        if (weaponFollower != null)
            weaponFollower.SampleGreatSwordAttack(AttackComboIndex, phase, normalizedTime, startup, activeDuration, recovery, dashAttack);
    }

    private int GetComboHitCount()
    {
        return Weapon.CurrentWeaponType switch
        {
            WeaponType.Dagger => 3,
            WeaponType.StraightSword => 4,
            WeaponType.GreatSword => 2,
            _ => 1
        };
    }
}
