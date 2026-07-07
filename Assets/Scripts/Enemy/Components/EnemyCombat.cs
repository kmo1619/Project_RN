using UnityEngine;

public class EnemyCombat : MonoBehaviour, IParryable
{
    private EnemyController controller;

    private int currentPoise;

    private int currentKnockdownGauge;

    [Header("Attack")]
    public Transform attackPoint;

    public LayerMask playerLayer;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    private void Start()
    {
        currentPoise = controller.stats.poise;
    }

    public void OnParried()
    {
        AddKnockdownGauge(controller.stats.parryKnockdownPower);
    }

    public void PerformAttack()
    {
        Vector2 origin = attackPoint != null
            ? (Vector2)attackPoint.position
            : (Vector2)controller.transform.position;

        Collider2D hit = Physics2D.OverlapCircle(
            origin,
            controller.stats.attackRange,
            playerLayer);

        if (hit != null)
        {
            IParryReceiver parryReceiver =
                hit.GetComponent<IParryReceiver>();

            if (parryReceiver != null &&
                parryReceiver.TryParry(controller.stats.attackStaggerPower))
            {
                OnParried();
                Debug.Log($"{name} Attack — Parried");
                return;
            }

            IDamageable damageable =
                hit.GetComponent<IDamageable>();

            damageable?.TakeDamage(
                controller.stats.attackDamage,
                controller.stats.attackStaggerPower);
        }

        Debug.Log($"{name} Attack");
    }

    public void ApplyStagger(int staggerPower)
    {
        bool hitStun = CheckHitStun(staggerPower);

#if UNITY_EDITOR
        string result = hitStun ? "HitStun" : "NoHitStun";
        Debug.Log(
            $"[POISE][ENEMY] Stagger:{staggerPower} " +
            $"Poise:{controller.stats.poise} " +
            $"Result:{result}");
#endif

        if (hitStun)
        {
            controller.EnterHitStun();
            return;
        }

        AddKnockdownGauge(staggerPower);
    }

    public bool CheckHitStun(int staggerPower)
    {
        return staggerPower >= controller.stats.poise;
    }

    public void AddKnockdownGauge(int amount)
    {
        currentKnockdownGauge += amount;

#if UNITY_EDITOR
        Debug.Log(
            $"[KNOCKDOWN][ENEMY] " +
            $"{currentKnockdownGauge}/{controller.stats.maxKnockdownGauge}");
#endif

        if (currentKnockdownGauge
            >= controller.stats.maxKnockdownGauge)
        {
            TriggerKnockdown();
        }
    }

    private void TriggerKnockdown()
    {
#if UNITY_EDITOR
        Debug.Log("[KNOCKDOWN][ENEMY] Triggered");
#endif

        currentKnockdownGauge = 0;
        controller.EnterKnockdown();
    }

    public void ResetPoise()
    {
        currentPoise = controller.stats.poise;
    }
}
