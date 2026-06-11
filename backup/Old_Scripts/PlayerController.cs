using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStateMachine stateMachine;

    private PlayerAttack playerAttack;
    private PlayerParry playerParry;
    private PlayerDash playerDash;

    void Start()
    {
        stateMachine =
            GetComponent<PlayerStateMachine>();

        playerAttack =
            GetComponent<PlayerAttack>();

        playerParry =
            GetComponent<PlayerParry>();

        playerDash =
            GetComponent<PlayerDash>();
    }

    void Update()
    {
        if (
            stateMachine.IsState(PlayerState.Dead)
        )
        {
            return;
        }

        // 공격
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (playerAttack != null)
            {
                playerAttack.StartAttack();
            }
        }

        // 패링
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (playerParry != null)
            {
                playerParry.StartParry();
            }
        }

        // 대시
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (playerDash != null)
            {
                playerDash.StartDash();
            }
        }
    }
}