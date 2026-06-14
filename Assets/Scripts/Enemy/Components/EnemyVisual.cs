using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyVisual : MonoBehaviour
{
    [Header("Hit Feedback")]
    public Color hitStunColor = Color.red;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void ShowHitStun()
    {
        spriteRenderer.color = hitStunColor;
    }

    public void ClearHitStun()
    {
        spriteRenderer.color = originalColor;
    }
}
