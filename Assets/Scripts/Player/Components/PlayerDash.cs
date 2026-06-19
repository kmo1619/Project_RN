using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private float cooldownTimer;

    public bool CanDash => cooldownTimer <= 0f;

    public float DashDirection { get; private set; }

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    public void Prepare(float direction)
    {
        DashDirection = direction;
    }

    public void UseDash(float cooldown)
    {
        cooldownTimer = cooldown;
    }
}
