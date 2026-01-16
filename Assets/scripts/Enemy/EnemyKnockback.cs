using UnityEngine;

public class EnemyKnockback : MonoBehaviour
{
    // --- 설정값 (인스펙터에서 적마다 다르게 설정 가능) ---
    [SerializeField]
    private float KnockbackForceX = 5.0f;      // 적이 뒤로 밀려나는 힘

    [SerializeField]
    private float KnockbackForceY = 3.0f;      // 적이 위로 솟구치는 힘

    [SerializeField]
    private float KnockbackDuration = 0.2f;    // 넉백이 지속되는 시간

    // --- 내부 상태 변수 ---
    private float KnockbackTimer = 0.0f;
    private bool isKnockbackActive = false;

    private Vector2 KnockbackVelocity = Vector2.zero;

    // 매 프레임마다 실행
    void Update()
    {
        if (isKnockbackActive == true)
        {
            // 1. 넉백 시간 관리
            KnockbackTimer -= Time.deltaTime;

            // 2. 실제 위치 이동 처리 (Transform 방식) = 현재 위치 + (속력 * 시간)
            transform.position += (Vector3)KnockbackVelocity * Time.deltaTime;

            // 3. 타이머 종료 체크
            if (KnockbackTimer <= 0.0f)
            {
                isKnockbackActive = false;
                KnockbackTimer = 0.0f;
                KnockbackVelocity = Vector2.zero;
            }
        }
    }

    // 현재 넉백 상태인지 확인 (EnemyPatrol에서 이동을 멈추기 위해 호출)
    public bool IsKnockbackActive()
    {
        return isKnockbackActive;
    }

    // 넉백 발생 함수 (플레이어에게 맞았을 때 호출)
    public void ApplyKnockback(Vector2 direction)
    {
        isKnockbackActive = true;
        KnockbackTimer = KnockbackDuration;

        // X축 방향 결정
        float xSign = 1.0f;
        if (direction.x < 0.0f)
        {
            xSign = -1.0f;
        }

        // 넉백 속도 벡터 생성
        float vx = xSign * KnockbackForceX;
        float vy = KnockbackForceY;

        KnockbackVelocity = new Vector2(vx, vy);
    }
}