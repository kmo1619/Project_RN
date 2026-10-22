using UnityEngine;

/// <summary>
/// Plays the knight-only idle, move, and attack sprites.
/// Weapon sprites and weapon physics are handled separately.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private float idleFramesPerSecond = 4f;
    [SerializeField] private float moveFramesPerSecond = 8f;

    private const int IdleFrameCount = 5;
    private const int MoveFrameCount = 4;
    private const int AttackFrameCount = 3;

    private readonly Sprite[] idleFrames = new Sprite[IdleFrameCount];
    private readonly Sprite[] moveFrames = new Sprite[MoveFrameCount];
    private readonly Sprite[] attackFrames = new Sprite[AttackFrameCount];

    private SpriteRenderer spriteRenderer;
    private bool isMoving;
    private bool isAttackingVisual;
    private float animationTimer;
    private int frameIndex;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        LoadFrames("Idle", idleFrames);
        LoadFrames("Move", moveFrames);
        LoadFrames("Attack", attackFrames);
        ApplyCurrentSprite();
    }

    private static void LoadFrames(string animationName, Sprite[] frames)
    {
        for (int i = 0; i < frames.Length; i++)
        {
            frames[i] = Resources.Load<Sprite>($"PlayerKnight/{animationName}_{i:00}");
            if (frames[i] == null)
                Debug.LogWarning($"Missing knight {animationName} frame {i:00} in Resources/PlayerKnight.");
        }
    }

    // Kept for PlayerController compatibility. Equipment no longer swaps the knight body sprite.
    public void SetWeapon(WeaponType weapon)
    {
    }

    public void SetState(IState state)
    {
        if (state is PlayerAttackStartupState || state is PlayerAttackActiveState || state is PlayerAttackRecoveryState)
        {
            isMoving = false;
            isAttackingVisual = true;
            return;
        }

        bool shouldMove = state is PlayerMoveState;
        if (isAttackingVisual || isMoving != shouldMove)
        {
            isAttackingVisual = false;
            isMoving = shouldMove;
            frameIndex = 0;
            animationTimer = 0f;
            ApplyCurrentSprite();
        }
    }

    public void SetAttackFrame(WeaponType weapon, int hitIndex, string phase, float normalizedTime)
    {
        // Wind up through frames 1-2-3, hold frame 3 during the active hit,
        // then return through frames 3-2-1 during recovery.
        int phaseFrame = Mathf.Clamp(
            Mathf.FloorToInt(Mathf.Clamp01(normalizedTime) * AttackFrameCount),
            0,
            AttackFrameCount - 1);

        int index = phase switch
        {
            "Startup" => phaseFrame,
            "Attack" => AttackFrameCount - 1,
            "Recovery" => AttackFrameCount - 1 - phaseFrame,
            _ => 0
        };

        if (attackFrames[index] != null)
            spriteRenderer.sprite = attackFrames[index];
    }

    public void Tick()
    {
        if (isAttackingVisual)
            return;

        Sprite[] activeFrames = isMoving ? moveFrames : idleFrames;
        float framesPerSecond = isMoving ? moveFramesPerSecond : idleFramesPerSecond;
        animationTimer += Time.deltaTime;
        float frameDuration = 1f / Mathf.Max(1f, framesPerSecond);
        while (animationTimer >= frameDuration)
        {
            animationTimer -= frameDuration;
            frameIndex = (frameIndex + 1) % activeFrames.Length;
            ApplyCurrentSprite();
        }
    }

    private void ApplyCurrentSprite()
    {
        if (spriteRenderer == null)
            return;

        Sprite[] frames = isMoving ? moveFrames : idleFrames;
        if (frames[frameIndex] != null)
            spriteRenderer.sprite = frames[frameIndex];
    }
}
