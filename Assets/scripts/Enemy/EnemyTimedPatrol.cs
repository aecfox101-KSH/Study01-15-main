using UnityEngine;

public class EnemyTimedPatrol : MonoBehaviour
{
    [Header("- 이동 설정 -")]
    public float moveSpeed = 2.0f;
    public float changeDirectionTime = 3.0f; // [미션 1] 이동 지속 시간
    public float waitTime = 1.5f;           // [미션 2] 대기 시간

    private float timer = 0.0f;      // 시간 체크용 타이머
    private bool isWaiting = false;  // 현재 대기 중인지 여부
    private bool moveRight = true;   // 현재 오른쪽 이동 여부

    [Header("- 장애물 감지 설정 -")]
    public Transform detectionPoint;      // 적의 앞쪽 하단 빈 오브젝트
    public float detectionDistance = 0.5f; // 레이캐스트 길이
    public LayerMask groundLayer;         // 바닥/벽 레이어

    [Header("- 컴포넌트 연결 -")]
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private PlayerKnockback knockback;

    void Start()
    {
        moveRight = true;
        timer = 0.0f;
    }

    void Update()
    {
        // 1. 넉백 체크: 넉백 중이면 아래의 이동/대기 로직을 모두 건너뜁니다.
        if (knockback != null && knockback.IsKnockbackActive())
        {
            return;
        }

        // 2. 시간 흐름 계산
        timer += Time.deltaTime;

        // 3. 상태에 따른 행동 수행
        if (isWaiting == true)
        {
            UpdateWait();
        }
        else
        {
            UpdateMove();
        }
    }

    // --- 이동 상태 로직 ---
    void UpdateMove()
    {
        // [장애물 체크] 앞이 막혔거나 낭떠러지면 대기 상태로 전환
        if (CheckObstacles() == true)
        {
            StartWaiting();
            return;
        }

        float directionX = 0.0f;

        if (moveRight == true)
        {
            directionX = 1.0f;
        }
        else
        {
            directionX = -1.0f;
        }

        // 이동 처리 (Transform 방식)
        float distance = moveSpeed * Time.deltaTime * directionX;
        transform.position += new Vector3(distance, 0.0f, 0.0f);

        if (animator != null)
        {
            animator.SetBool("move", true);
        }

        // [미션 1] 지정된 시간이 지나면 대기 상태로 전환
        if (timer >= changeDirectionTime)
        {
            StartWaiting();
        }
    }
    bool CheckObstacles()
    {
        if (detectionPoint == null)
        {
            return false;
        }

        // 1. 낭떠러지 감지 (아래 방향)
        RaycastHit2D groundInfo = Physics2D.Raycast(detectionPoint.position, Vector2.down, detectionDistance, groundLayer);

        // 2. 벽 감지 (앞 방향)
        Vector2 forwardDir;
        if (moveRight == true)
        {
            forwardDir = Vector2.right;
        }
        else
        {
            forwardDir = Vector2.left;
        }

        RaycastHit2D wallInfo = Physics2D.Raycast(detectionPoint.position, forwardDir, 0.3f, groundLayer);

        // 결과 판단
        if (groundInfo.collider == null) // 바닥이 없으면 (낭떠러지)
        {
            return true;
        }

        if (wallInfo.collider != null) // 벽에 닿았으면
        {
            return true;
        }

        return false;
    }


    // --- 대기 시작 설정 ---
    void StartWaiting()
    {
        timer = 0f;
        isWaiting = true;

        if (animator != null)
        {
            animator.SetBool("move", false); // 대기 중엔 이동 애니메이션 끔
        }

        // 방향 전환을 미리 해둡니다.
        TurnAround();
    }

    // --- 대기 상태 로직 ---
    void UpdateWait()
    {
        // [미션 2] 대기 시간이 지나면 다시 이동 시작
        if (timer >= waitTime)
        {
            timer = 0f;
            isWaiting = false;
        }
    }

    // --- 물리 엔진 기반 넉백 처리 ---
    private void FixedUpdate()
    {
        if (knockback != null && knockback.IsKnockbackActive() == true)
        {
            // 넉백 중일 때만 물리 속도(Velocity)를 직접 제어합니다.
            rb.linearVelocity = knockback.GetKnockbackVelocity();
        }
    }

    // --- 방향 전환 기능 ---
    void TurnAround()
    {
        if (moveRight == true)
        {
            moveRight = false;
        }
        else
        {
            moveRight = true;
        }

        if (spriteRenderer != null)
        {
            if (moveRight == true)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
        // [핵심] 감지 포인트의 위치도 반대로 옮겨줘야 반대편 낭떠러지를 감지합니다.
        if (detectionPoint != null)
        {
            Vector3 localPos = detectionPoint.localPosition;
            localPos.x = localPos.x * -1; // 현재 X좌표를 반전 (예: 0.5 -> -0.5)
            detectionPoint.localPosition = localPos;
        }
    }
}