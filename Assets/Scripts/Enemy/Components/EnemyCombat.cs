using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private EnemyController controller;

    private int currentPoise;

    private int currentKnockdownGauge;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    private void Start()
    {
        currentPoise = controller.stats.poise;
    }

    public void PerformAttack()
    {
        Debug.Log($"{name} Attack");
    }

    public void ApplyStagger(int staggerPower)
    {
        if (CheckHitStun(staggerPower))
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

        Debug.Log(
            $"Knockdown Gauge : " +
            $"{currentKnockdownGauge} / " +
            $"{controller.stats.maxKnockdownGauge}");

        if (currentKnockdownGauge
            >= controller.stats.maxKnockdownGauge)
        {
            TriggerKnockdown();
        }
    }

    private void TriggerKnockdown()
    {
        Debug.Log($"{name} Knockdown");

        currentKnockdownGauge = 0;
    }

    public void ResetPoise()
    {
        currentPoise = controller.stats.poise;
    }
}
