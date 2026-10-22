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
    private Transform visualRoot;

    private SpriteRenderer greatSword;
    private SpriteRenderer daggerNear;
    private SpriteRenderer daggerFar;
    private SpriteRenderer straightSword;
    private SpriteRenderer shield;

    private Vector3 followVelocity;
    private bool attackPoseOverride;
    private Vector2 attackPoseOffsetPixels;
    private float attackPoseAngle;
    private readonly Dictionary<SpriteRenderer, Vector3> partFollowVelocities = new();
    private readonly HashSet<SpriteRenderer> initializedParts = new();

    public Transform ActiveWeaponTransform
    {
        get
        {
            if (playerWeapon == null)
                return null;

            return playerWeapon.CurrentWeaponType switch
            {
                WeaponType.GreatSword => greatSword != null ? greatSword.transform : null,
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

        visualRoot = new GameObject("PlayerWeaponVisuals").transform;
        visualRoot.position = transform.position;
        visualRoot.rotation = Quaternion.identity;
        visualRoot.localScale = Vector3.one;

        greatSword = CreatePart("GreatSword", "PlayerKnight/GreatSword", -1);
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
        SetPartPose(greatSword, -11f * facing, 3f, colliderBottomOffset);
        SetPartPose(daggerNear, -7f * facing, 16f, colliderBottomOffset);
        SetPartPose(daggerFar, -12f * facing, 16f, colliderBottomOffset);
        SetPartPose(straightSword, -9f * facing, 7f, colliderBottomOffset);
        SetPartPose(shield, 12f * facing, 13f, colliderBottomOffset);
    }

    private void SetPartPose(SpriteRenderer part, float xPixels, float bottomClearancePixels, float colliderBottomOffset)
    {
        if (part == null || part.sprite == null)
            return;

        float y = colliderBottomOffset
            + bottomClearancePixels / referencePixelsPerUnit
            + part.sprite.bounds.extents.y;
        Vector3 targetPosition = new Vector3(xPixels / referencePixelsPerUnit, y, 0f);
        if (initializedParts.Add(part))
        {
            part.transform.localPosition = targetPosition;
        }
        else
        {
            Vector3 velocity = partFollowVelocities.TryGetValue(part, out Vector3 currentVelocity)
                ? currentVelocity
                : Vector3.zero;
            part.transform.localPosition = Vector3.SmoothDamp(
                part.transform.localPosition,
                targetPosition,
                ref velocity,
                followSmoothTime,
                Mathf.Infinity,
                Time.deltaTime);
            partFollowVelocities[part] = velocity;
        }
        part.transform.localRotation = Quaternion.identity;
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
        SetPartFacing(greatSword, facing);
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
        ApplyFollowLayout();
    }

    private void ApplyAttackPose()
    {
        ApplyFollowLayout();
        Transform weapon = ActiveWeaponTransform;
        if (weapon == null)
            return;

        weapon.localPosition += new Vector3(
            attackPoseOffsetPixels.x / referencePixelsPerUnit,
            attackPoseOffsetPixels.y / referencePixelsPerUnit,
            0f);
        weapon.localRotation = Quaternion.Euler(0f, 0f, attackPoseAngle * GetFacingDirection());
    }

    private void OnDestroy()
    {
        if (visualRoot != null)
            Destroy(visualRoot.gameObject);
    }
}
