using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // =========================
    // 따라갈 대상
    // =========================
    public Transform target; // 플레이어 Transform

    // =========================
    // 카메라 이동 속도
    // 값이 클수록 더 빠르게 따라감
    // =========================
    public float smoothSpeed = 5f;

    void LateUpdate()
    {
        // =========================
        // 목표 위치 계산
        //
        // X축은 플레이어를 따라감
        // Y축은 현재 카메라 위치 유지
        // Z축은 카메라 위치(-10) 고정
        // =========================
        Vector3 targetPosition = new Vector3(
            target.position.x,
            transform.position.y,
            -10
        );

        // =========================
        // 부드럽게 이동
        //
        // 현재 위치에서 목표 위치까지
        // Lerp를 사용해 자연스럽게 이동
        // =========================
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}