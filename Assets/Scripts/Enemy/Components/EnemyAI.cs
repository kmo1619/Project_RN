using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private EnemyController controller;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }

    public bool CanDetectPlayer()
    {
        if (controller.GetPlayer() == null)
            return false;

        return controller.DistanceToPlayer()
            <= controller.stats.detectRange;
    }

    public bool IsInAttackRange()
    {
        if (controller.GetPlayer() == null)
            return false;

        return controller.DistanceToPlayer()
            <= controller.stats.attackRange;
    }

    public Transform GetPlayer()
    {
        return controller.GetPlayer();
    }
}