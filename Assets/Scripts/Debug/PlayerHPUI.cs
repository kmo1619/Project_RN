using System;
using TMPro;
using UnityEngine;

public class PlayerHPUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;

    [SerializeField] private TextMeshProUGUI hpText;

    [SerializeField] private TextMeshProUGUI hpBar;

    private const int BarLength = 10;

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged += RefreshUI;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= RefreshUI;
    }

    private void Start()
    {
        if (playerHealth != null)
            RefreshUI(playerHealth.CurrentHealth, playerHealth.maxHealth);
    }

    private void RefreshUI(int current, int max)
    {
        if (hpText != null)
            hpText.text = $"HP {current} / {max}";

        if (hpBar != null)
        {
            float ratio = max > 0 ? (float)current / max : 0f;
            int filled = Mathf.Clamp(
                Mathf.RoundToInt(ratio * BarLength),
                0,
                BarLength);

            hpBar.text =
                new string('█', filled) +
                new string('░', BarLength - filled);
        }
    }
}
