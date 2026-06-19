using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public int maxHealth = 100;

    [Header("Invincibility")]
    public float invincibleDuration = 0.3f;

    public int CurrentHealth { get; private set; }

    public bool isInvincible => invincibleTimer > 0f || forcedInvincible;

    public event Action<int, int> OnHealthChanged;

    private float invincibleTimer;

    private bool forcedInvincible;

    private bool isDead;

    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        CurrentHealth = maxHealth;
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    private void Update()
    {
        if (invincibleTimer > 0f)
        {
            invincibleTimer -= Time.deltaTime;
        }
    }

    public void SetInvincible(bool value)
    {
        forcedInvincible = value;
    }

    public void TakeDamage(int damage, int staggerPower)
    {
        if (isInvincible || isDead)
            return;

        int hpBefore = CurrentHealth;
        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);

#if UNITY_EDITOR
        Debug.Log($"[DAMAGE] {hpBefore}->{CurrentHealth} ({CurrentHealth - hpBefore})");
#endif

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0)
        {
            isDead = true;
            controller.StateMachine.ChangeState(
                controller.DeadState);
            return;
        }

        invincibleTimer = invincibleDuration;

#if UNITY_EDITOR
        string result = staggerPower > controller.currentPoise
            ? "HitStun"
            : "NoHitStun";
        Debug.Log(
            $"[POISE] Stagger:{staggerPower} " +
            $"Poise:{controller.currentPoise} " +
            $"Result:{result}");
#endif

        if (staggerPower > controller.currentPoise)
        {
            controller.EnterHitStun();
        }
    }
}
