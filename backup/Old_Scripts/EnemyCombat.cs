using UnityEngine;
using System.Collections;

public class EnemyCombat : MonoBehaviour
{
    // =========================
    // 강인도 설정
    // 공격의 경직 수치가
    // 강인도보다 높으면 경직 발생
    // =========================
    [Header("Poise")]
    public int poise = 10;

    // =========================
    // 경직 지속 시간
    // =========================
    [Header("Hit Stun")]
    public float hitStunDuration = 0.15f;

    // =========================
    // 넉다운 게이지 설정
    // =========================
    [Header("Knockdown")]
    public int maxKnockdownGauge = 100;

    // 현재 넉다운 게이지
    private int currentKnockdownGauge;

    // 경직 상태 여부
    private bool isHitStunned;

    // 넉다운 상태 여부
    private bool isKnockedDown;

    // 적 스프라이트
    private SpriteRenderer spriteRenderer;

    // 원래 스프라이트 색상
    private Color originalColor;

    void Start()
    {
        // SpriteRenderer 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 원래 색상 저장
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    // =========================
    // 현재 경직 상태 반환
    // =========================
    public bool IsHitStunned()
    {
        return isHitStunned;
    }

    // =========================
    // 현재 넉다운 상태 반환
    // =========================
    public bool IsKnockedDown()
    {
        return isKnockedDown;
    }

    // =========================
    // 공격에 의한 경직 처리
    // =========================
    public void TryHitStun(int staggerPower)
    {
        // 경직 수치가 강인도보다 높으면 경직
        if (staggerPower > poise)
        {
            StartCoroutine(HitStunRoutine());
        }
    }

    // =========================
    // 패링 성공 시
    // 넉다운 게이지 누적
    // =========================
    public void AddKnockdownGauge(int amount)
    {
        currentKnockdownGauge += amount;

        Debug.Log(
            gameObject.name +
            " Knockdown Gauge : " +
            currentKnockdownGauge +
            " / " +
            maxKnockdownGauge
        );

        // 게이지가 최대치 이상이면 넉다운
        if (currentKnockdownGauge >= maxKnockdownGauge)
        {
            StartCoroutine(KnockdownRoutine());
        }
    }

    // =========================
    // 경직 처리
    // =========================
    private IEnumerator HitStunRoutine()
    {
        // 이미 경직 중이면 중복 실행 방지
        if (isHitStunned)
            yield break;

        // 경직 시작
        isHitStunned = true;

        // 현재 공격 중이면 공격 취소
        EnemyAI enemyAI = GetComponent<EnemyAI>();

        if (enemyAI != null)
        {
            enemyAI.CancelAttack();
        }

        Debug.Log(
            gameObject.name +
            " Hit Stunned"
        );

        // 피격 섬광 효과
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }

        // 경직 시간 대기
        yield return new WaitForSeconds(
            hitStunDuration
        );

        // 원래 색상 복구
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        // 경직 종료
        isHitStunned = false;
    }

    // =========================
    // 넉다운 처리
    // =========================
    private IEnumerator KnockdownRoutine()
    {
        // 이미 넉다운 상태면 중복 실행 방지
        if (isKnockedDown)
            yield break;

        // 넉다운 시작
        isKnockedDown = true;

        // 넉다운 게이지 초기화
        currentKnockdownGauge = 0;

        Debug.Log(
            gameObject.name +
            " Knocked Down"
        );

        // 넉다운 상태 표시
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.gray;
        }

        // 넉다운 지속 시간
        yield return new WaitForSeconds(2f);

        // 원래 색상 복구
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        // 넉다운 종료
        isKnockedDown = false;

        Debug.Log(
            gameObject.name +
            " Recovered"
        );
    }
}