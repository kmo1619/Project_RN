using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays weapon parts in a separate visual rig that eases behind the player.
/// This rig has no collider and does not change combat hit detection.
/// </summary>
public class PlayerWeaponFollower : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float followSmoothTime = 0.08f;
    [SerializeField, Min(1f)] private float referencePixelsPerUnit = 32f;

    private PlayerWeapon playerWeapon;
    private PlayerMovement playerMovement;
    private Collider2D playerCollider;
    [SerializeField] private Transform visualRoot;
    [SerializeField] private Transform greatSwordAnchor;
    [SerializeField] private Transform greatSwordPivot;
    [SerializeField] private AnimationClip greatSwordAttackClip;
    [SerializeField] private AnimationClip greatSwordAttackClip2;

    private SpriteRenderer greatSword;
    private SpriteRenderer daggerNear;
    private SpriteRenderer daggerFar;
    private SpriteRenderer straightSword;
    private SpriteRenderer shield;

    private Vector3 followVelocity;
    private bool attackPoseOverride;
    private Vector2 attackPoseOffsetPixels;
    private float attackPoseAngle;
    private bool ownsVisualRoot;
    private readonly Dictionary<Transform, Vector3> partFollowVelocities = new();
    private readonly HashSet<Transform> initializedParts = new();

    public Transform ActiveWeaponTransform
    {
        get
        {
            if (playerWeapon == null)
                return null;

            return playerWeapon.CurrentWeaponType switch
            {
                WeaponType.GreatSword => greatSwordPivot,
                WeaponType.Dagger => daggerNear != null ? daggerNear.transform : null,
                _ => straightSword != null ? straightSword.transform : null
            };
        }
    }

    public Transform ShieldTransform => shield != null ? shield.transform : null;

    private void Awake()
    {
        playerWeapon = GetComponent<PlayerWeapon>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<Collider2D>();

        if (visualRoot == null)
        {
            visualRoot = new GameObject("PlayerWeaponVisuals").transform;
            ownsVisualRoot = true;
        }
        visualRoot.position = transform.position;
        visualRoot.rotation = Quaternion.identity;
        visualRoot.localScale = Vector3.one;

        if (greatSwordAnchor == null)
        {
            greatSwordAnchor = new GameObject("GreatSwordAnchor").transform;
            greatSwordAnchor.SetParent(visualRoot, false);
        }
        if (greatSwordPivot == null)
        {
            greatSwordPivot = new GameObject("GreatSwordPivot").transform;
            greatSwordPivot.SetParent(greatSwordAnchor, false);
        }
        greatSword = greatSwordPivot.GetComponentInChildren<SpriteRenderer>();
        // The attack clip is sampled directly by the player combat states so its
        // authored motion follows the complete startup/active/recovery timeline.
        // Keep the Animator from also writing to the same pivot.
        var pivotAnimator = greatSwordPivot.GetComponent<Animator>();
        if (pivotAnimator != null)
            pivotAnimator.enabled = false;
        if (greatSword == null)
        {
            var swordSprite = Resources.Load<Sprite>("PlayerKnight/GreatSword");
            if (swordSprite != null)
            {
                var swordObject = new GameObject("GreatSwordSprite");
                swordObject.transform.SetParent(greatSwordPivot, false);
                greatSword = swordObject.AddComponent<SpriteRenderer>();
                greatSword.sprite = swordSprite;
                greatSword.sortingOrder = -1;
            }
        }
        daggerNear = CreatePart("DaggerNear", "PlayerKnight/Dagger", -1);
        daggerFar = CreatePart("DaggerFar", "PlayerKnight/Dagger", -1);
        straightSword = CreatePart("StraightSword", "PlayerKnight/StraightSword", -1);
        shield = CreatePart("Shield", "PlayerKnight/Shield", 1);

        SetActiveWeapon();
        ApplyFollowLayout();
    }

    private SpriteRenderer CreatePart(string partName, string resourcePath, int sortingOrder)
    {
        Sprite sprite = Resources.Load<Sprite>(resourcePath);
        if (sprite == null)
        {
            Debug.LogWarning($"Missing weapon sprite at Resources/{resourcePath}.", this);
            return null;
        }

        var part = new GameObject(partName);
        part.transform.SetParent(visualRoot, false);
        var renderer = part.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sortingOrder = sortingOrder;
        return renderer;
    }

    private void LateUpdate()
    {
        if (visualRoot == null)
            return;

        Vector3 targetPosition = transform.position;
        visualRoot.position = Vector3.SmoothDamp(
            visualRoot.position,
            targetPosition,
            ref followVelocity,
            followSmoothTime,
            Mathf.Infinity,
            Time.deltaTime);

        visualRoot.localScale = Vector3.one;
        ApplyPartFacing(GetFacingDirection());
        SetActiveWeapon();

        if (!attackPoseOverride)
            ApplyFollowLayout();
        else
            ApplyAttackPose();
    }

    public void SetWeapon(WeaponType weaponType)
    {
        SetActiveWeapon(weaponType);
    }

    private void SetActiveWeapon()
    {
        WeaponType type = playerWeapon != null
            ? playerWeapon.CurrentWeaponType
            : WeaponType.StraightSword;

        SetActiveWeapon(type);
    }

    private void SetActiveWeapon(WeaponType type)
    {
        if (greatSword != null) greatSword.enabled = type == WeaponType.GreatSword;
        if (daggerNear != null) daggerNear.enabled = type == WeaponType.Dagger;
        if (daggerFar != null) daggerFar.enabled = type == WeaponType.Dagger;
        if (straightSword != null) straightSword.enabled = type == WeaponType.StraightSword;
        if (shield != null) shield.enabled = type == WeaponType.StraightSword;
    }

    private void ApplyFollowLayout()
    {
        float facing = GetFacingDirection();
        float colliderBottomOffset = playerCollider != null
            ? playerCollider.bounds.min.y - transform.position.y
            : -0.5f;

        // X is measured from the player origin in 32x32 character pixels.
        // Keep the visual root unflipped so each part can ease across when facing changes.
        SetPartPose(greatSwordAnchor, greatSword, -11f * facing, 3f, colliderBottomOffset);
        SetPartPose(daggerNear.transform, daggerNear, -7f * facing, 16f, colliderBottomOffset);
        SetPartPose(daggerFar.transform, daggerFar, -12f * facing, 16f, colliderBottomOffset);
        SetPartPose(straightSword.transform, straightSword, -9f * facing, 7f, colliderBottomOffset);
        SetPartPose(shield.transform, shield, 12f * facing, 13f, colliderBottomOffset);
    }

    private void SetPartPose(Transform partTransform, SpriteRenderer partRenderer, float xPixels, float bottomClearancePixels, float colliderBottomOffset)
    {
        if (partTransform == null || partRenderer == null || partRenderer.sprite == null)
            return;

        float spriteOffsetY = partRenderer.transform == partTransform
            ? 0f
            : partRenderer.transform.localPosition.y;
        float spriteBottomOffset = spriteOffsetY + partRenderer.sprite.bounds.min.y;
        float y = colliderBottomOffset
            + bottomClearancePixels / referencePixelsPerUnit
            - spriteBottomOffset;
        Vector3 targetPosition = new Vector3(xPixels / referencePixelsPerUnit, y, 0f);
        if (initializedParts.Add(partTransform))
        {
            partTransform.localPosition = targetPosition;
        }
        else
        {
            Vector3 velocity = partFollowVelocities.TryGetValue(partTransform, out Vector3 currentVelocity)
                ? currentVelocity
                : Vector3.zero;
            partTransform.localPosition = Vector3.SmoothDamp(
                partTransform.localPosition,
                targetPosition,
                ref velocity,
                followSmoothTime,
                Mathf.Infinity,
                Time.deltaTime);
            partFollowVelocities[partTransform] = velocity;
        }
        partTransform.localRotation = Quaternion.identity;
    }

    private float GetFacingDirection()
    {
        float facing = playerMovement != null
            ? playerMovement.FacingDirection
            : Mathf.Sign(transform.localScale.x);
        return facing == 0f ? 1f : Mathf.Sign(facing);
    }

    private void ApplyPartFacing(float facing)
    {
        SetPartFacing(greatSwordAnchor, facing);
        SetPartFacing(daggerNear, facing);
        SetPartFacing(daggerFar, facing);
        SetPartFacing(straightSword, facing);
        SetPartFacing(shield, facing);
    }

    private static void SetPartFacing(SpriteRenderer part, float facing)
    {
        if (part != null)
            part.transform.localScale = new Vector3(facing, 1f, 1f);
    }

    private static void SetPartFacing(Transform part, float facing)
    {
        if (part != null)
            part.localScale = new Vector3(facing, 1f, 1f);
    }

    /// <summary>
    /// Overrides the active weapon's follow pose. Offsets are relative to its rest pose,
    /// in character pixels; call ClearAttackPoseOverride when the attack motion ends.
    /// </summary>
    public void SetAttackPose(Vector2 offsetPixels, float rotationDegrees)
    {
        attackPoseOverride = true;
        attackPoseOffsetPixels = offsetPixels;
        attackPoseAngle = rotationDegrees;
        ApplyAttackPose();
    }

    public void ClearAttackPoseOverride()
    {
        attackPoseOverride = false;
        if (greatSwordPivot != null)
        {
            greatSwordPivot.localPosition = Vector3.zero;
            greatSwordPivot.localRotation = Quaternion.identity;
        }
        ApplyFollowLayout();
    }

    public void SampleGreatSwordAttack(int hitIndex, string phase, float normalizedTime, float activeDuration)
    {
        AnimationClip attackClip = hitIndex <= 0 ? greatSwordAttackClip : greatSwordAttackClip2;
        if (attackClip == null || greatSwordPivot == null || playerWeapon == null ||
            playerWeapon.CurrentWeaponType != WeaponType.GreatSword)
            return;

        WeaponData weapon = playerWeapon.CurrentWeapon;
        float startup = Mathf.Max(0.01f, weapon.attackStartup);
        float active = Mathf.Max(0.01f, activeDuration);
        float recovery = Mathf.Max(0.01f, weapon.attackRecovery);
        float total = startup + active + recovery;
        float elapsed = phase switch
        {
            "Startup" => Mathf.Clamp01(normalizedTime) * startup,
            "Attack" => startup + Mathf.Clamp01(normalizedTime) * active,
            "Recovery" => startup + active + Mathf.Clamp01(normalizedTime) * recovery,
            _ => 0f
        };

        attackClip.SampleAnimation(
            greatSwordPivot.gameObject,
            Mathf.Clamp(elapsed / total, 0f, 1f) * attackClip.length);
    }

    public void ClearGreatSwordAttack()
    {
        if (greatSwordPivot == null)
            return;

        greatSwordPivot.localPosition = Vector3.zero;
        greatSwordPivot.localRotation = Quaternion.identity;
    }

    private void ApplyAttackPose()
    {
        ApplyFollowLayout();
        Transform weapon = ActiveWeaponTransform;
        if (weapon == null)
            return;

        weapon.localPosition = new Vector3(
            attackPoseOffsetPixels.x / referencePixelsPerUnit,
            attackPoseOffsetPixels.y / referencePixelsPerUnit,
            0f);
        weapon.localRotation = Quaternion.Euler(0f, 0f, attackPoseAngle);
    }

    private void OnDestroy()
    {
        if (ownsVisualRoot && visualRoot != null)
            Destroy(visualRoot.gameObject);
    }
}
