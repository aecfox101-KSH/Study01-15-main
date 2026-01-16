using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    // --- 설정값 (에디터에서 수정 가능) ---
    [SerializeField]
    private float KnockbackForceX = 8.0f;     // X축(좌우)으로 튕겨나가는 힘의 세기

    [SerializeField]
    private float KnockbackForceY = 6.0f;     // Y축(위쪽)으로 튕겨나가는 힘의 세기

    [SerializeField]
    private float KnockbackDuration = 0.25f;  // 넉백이 지속될 시간

    // --- 내부 상태 변수 ---
    private float KnockbackTimer = 0.0f;      // 현재 남은 넉백 시간을 체크하는 타이머
    private bool isKnockbackActive = false;   // 현재 넉백 상태인지 여부

    private Vector2 KnockbackVelocity = Vector2.zero; // 외부(이동 스크립트 등)로 전달할 넉백 속도 값

    // 매 프레임마다 실행
    void Update()
    {
        // 넉백 상태일 때만 타이머를 작동시킴
        if (isKnockbackActive == true)
        {
            // 프레임당 흐른 시간만큼 타이머 감소
            KnockbackTimer -= Time.deltaTime;

            // 타이머가 0 이하가 되면 넉백 종료
            if (KnockbackTimer <= 0.0f)
            {
                isKnockbackActive = false;
                KnockbackTimer = 0.0f;

                // 넉백 속도를 0으로 초기화하여 이동 중지
                KnockbackVelocity = Vector2.zero;
            }
        }
    }

    // 현재 넉백 중인지 확인하는 함수 (외부에서 참조용)
    public bool IsKnockbackActive()
    {
        return isKnockbackActive;
    }

    // 계산된 넉백 속도를 가져오는 함수 (주로 PlayerController에서 최종 속도에 더할 때 사용)
    public Vector2 GetKnockbackVelocity()
    {
        return KnockbackVelocity;
    }

    // 넉백을 시작시키는 함수 (공격받은 시점에 호출됨)
    // direction: 플레이어가 넉백될 방향 정보
    public void ApplyKnockback(Vector2 direction)
    {
        isKnockbackActive = true;             // 넉백 상태 활성화
        KnockbackTimer = KnockbackDuration;   // 타이머를 설정된 지속 시간으로 초기화

        // 넉백 방향 결정 (입력받은 direction의 x값이 음수면 왼쪽, 양수면 오른쪽)
        float xSign = 1.0f;
        if (direction.x < 0.0f)
        {
            xSign = -1.0f;
        }

        // 미리 설정한 힘(ForceX, ForceY)에 방향을 곱해 속도 벡터 생성
        float vx = xSign * KnockbackForceX;   // 좌/우 속도
        float vy = KnockbackForceY;           // 위쪽으로 살짝 띄우는 속도

        KnockbackVelocity = new Vector2(vx, vy);
    }
}
