//
// 플레이어 상태 정의
//
public enum PlayerState
{
    // 아무 행동도 하지 않는 기본 상태
    Idle,

    // 이동 중인 상태
    Move,

    // 공격 선딜 상태
    // 공격 입력 후 실제 공격이 발생하기 전
    AttackStartup,

    // 공격 후딜 상태
    // 공격이 끝난 후 행동이 제한되는 시간
    AttackRecovery,

    // 패링 준비 상태
    // 패링 입력 후 활성되기 전
    ParryStartup,

    // 패링 활성 상태
    // 적 공격을 받아칠 수 있는 상태
    ParryActive,

    // 대시 상태
    Dash,

    // 사망 상태
    Dead
}