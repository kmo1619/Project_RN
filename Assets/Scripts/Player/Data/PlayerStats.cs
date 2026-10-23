using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    public float jumpForce = 10f;

    [Tooltip("Higher values make the player slow down sooner while rising.")]
    [Min(0.1f)] public float jumpRiseGravityMultiplier = 1.5f;

    [Tooltip("Higher values make the player fall faster after reaching the jump peak.")]
    [Min(0.1f)] public float jumpFallGravityMultiplier = 2.5f;

    [Header("Attack Movement")]
    public float attackStartupDeceleration = 30f;

    [Header("Dash")]
    public float dashSpeed = 12f;

    public float dashDistance = 5f;

    public float dashCooldown = 0.4f;

    [Header("Health")]
    public int maxHealth = 100;

    [Header("Poise")]
    public int idlePoise = 10;

    public int movePoise = 10;

    public int dashPoise = 5;

    [Header("Hit Stun")]
    public float hitStunDuration = 0.3f;
}
