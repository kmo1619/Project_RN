using UnityEngine;

[CreateAssetMenu(menuName = "Player/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    public float jumpForce = 10f;

    [Header("Dash")]
    public float dashSpeed = 12f;

    public float dashDuration = 0.15f;

    [Header("Health")]
    public int maxHealth = 100;
}