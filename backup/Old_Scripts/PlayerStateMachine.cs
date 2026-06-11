using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    // =========================
    // 현재 플레이어 상태
    // =========================
    [Header("Current State")]
    public PlayerState currentState =
        PlayerState.Idle;

    // =========================
    // 상태 변경
    // =========================
    public void ChangeState(
        PlayerState newState)
    {
        // 새로운 상태로 변경
        currentState = newState;

        // 현재 상태 출력
        Debug.Log(
            "Player State : " +
            currentState
        );
    }

    // =========================
    // 현재 상태 확인
    // =========================
    public bool IsState(
        PlayerState state)
    {
        return currentState == state;
    }
}