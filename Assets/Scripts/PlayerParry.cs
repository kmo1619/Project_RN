using UnityEngine;
using System.Collections;

public class PlayerParry : MonoBehaviour
{
    [Header("Parry Timing")]
    public float parryStartup = 0f;     // 패링 준비 시간
    public float parryDuration = 0.2f;  // 패링 활성 시간

    private bool isParrying;            // 패링 활성 여부
    private bool isPreparingParry;      // 패링 준비 중 여부

    // 패링 가능 여부 반환
    public bool IsParrying()
    {
        return isParrying;
    }

    void Update()
    {
        // F키 입력 시 패링 시작
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartParry();
        }
    }

    void StartParry()
    {
        // 이미 패링 중이면 무시
        if (isParrying)
            return;

        // 준비 중이면 무시
        if (isPreparingParry)
            return;

        StartCoroutine(ParryRoutine());
    }

    IEnumerator ParryRoutine()
    {
        // 패링 준비 시작
        isPreparingParry = true;

        Debug.Log("Parry Prepare");

        // 준비 시간 대기
        yield return new WaitForSeconds(parryStartup);

        // 패링 활성
        isPreparingParry = false;
        isParrying = true;

        Debug.Log("Parry Start");

        // 패링 지속
        yield return new WaitForSeconds(parryDuration);

        // 패링 종료
        isParrying = false;

        Debug.Log("Parry End");
    }

    // 패링 성공 시 패링 소모
    public void ConsumeParry()
    {
        StopAllCoroutines();

        isPreparingParry = false;
        isParrying = false;

        Debug.Log("Parry Consumed");
    }
}