using UnityEngine;

public class PlayerContraller : MonoBehaviour
{
    [Header("이동과 점프에 사용할 변수")]
    public float moveSpeed = 5.0f; // 캐릭터의 좌우 이동 속도를 결정하는 변수
    public float jumpSpeed = 8.0f; // 점프 시 위로 가해지는 속력의 크기

    [Header("물리 컴포넌트")]
    public Rigidbody2D rb; // 물리 엔진(중력, 마찰 등) 처리를 위한 컴포넌트 참조 변수

    [Header("애니메이션")]
    public Animator animator; // 캐릭터의 애니메이션 상태를 제어하는 컴포넌트 참조 변수

    [SerializeField]
    private PlayerKnockback knockback; // 외부 스크립트(넉백 기능)와의 상호작용을 위한 변수

    [SerializeField]
    private AudioSource jumpSound;

    private bool isGrounded = false; // 캐릭터가 현재 지면에 닿아 있는지 확인하는 상태 변수

    private float moveInput = 0.0f; // 사용자의 좌우 키 입력값(-1, 0, 1)을 담는 변수

    private bool jumpRequested = false; // 점프 입력을 받았는지 저장하는 일시적 변수


    // 매 프레임(초당 약 60회 이상)마다 호출되는 함수
    void Update()
    {
        // 1. 매 프레임 사용자의 입력과 상태 변화를 체크함
        HandleMoveInput();   // 좌우 입력 감지
        HandleJumpInput();   // 점프 입력 감지
        UpdateDirection();   // 이미지 방향 전환
        UpdateAnimation();   // 애니메이션 파라미터 갱신
    }

    /// <summary>
    ///  0.02초마다 고정 간격으로 호출.
    ///  물리나 중력 관련 처리를 할때 사용하는 것이 좋다.
    /// </summary>
    void FixedUpdate()
    {
        // 넉백 스크립트가 연결되어 있는지 확인
        if (knockback != null)
        {
            // 현재 캐릭터가 넉백 상태(피격 중)인지 확인
            bool isKnockback = knockback.IsKnockbackActive();

            if (isKnockback == true)
            {
                // 넉백 중이라면 조작을 무시하고 넉백 속도를 적용 후 함수 종료
                Vector2 kbVelocity = knockback.GetKnockbackVelocity();
                rb.linearVelocity = kbVelocity;
                return;
            }
        }

        // 넉백 상태가 아닐 때만 실제 이동 및 점프 물리 로직 실행
        ApplyMovement();
    }

    // 좌우 이동 입력을 받는 함수
    void HandleMoveInput()
    {
        // "Horizontal"축(A, D 키 혹은 화살표) 입력을 받아 moveInput에 저장
        // GetAxis는 값이 부드럽게 변하여 관성 있는 느낌을 줌
        moveInput = Input.GetAxis("Horizontal");
    }

    // 점프 입력을 받는 함수
    void HandleJumpInput()
    {
        // 스페이스바를 눌렀고, 현재 바닥 상태일 때만 점프 요청을 true로 설정
        if (Input.GetKeyDown(KeyCode.Space) == true && isGrounded == true)
        {
            jumpRequested = true;
        }
    }

    // 물리적 이동을 실제로 적용하는 함수
    void ApplyMovement()
    {
        // Rigidbody2D가 연결되어 있지 않으면 에러가 날 수 있으므로 체크 후 리턴
        if (rb == null)
        {
            return;
        }

        // 현재 캐릭터의 속도(Velocity)를 가져옴
        Vector2 velocity = rb.linearVelocity;

        // 입력값과 이동 속도를 곱해 X축(좌우) 목표 속도 계산
        float targetSpeedX = moveInput * moveSpeed;
        velocity.x = targetSpeedX;

        // 점프 요청이 있고 바닥 상태라면 Y축(위아래) 속도를 jumpSpeed로 변경
        if (jumpRequested == true && isGrounded == true)
        {
            velocity.y = jumpSpeed;

            // 점프를 시작했으므로 바닥 상태와 요청 상태를 초기화
            isGrounded = false;
            jumpRequested = false;

            Debug.Log("점프 실행!!");

            if(jumpSound != null)
            {
                jumpSound.Play();
                Debug.Log("점프 사운드 재생!!");
            }

        }

        // 변경된 최종 속도 값을 Rigidbody에 다시 적용
        rb.linearVelocity = velocity;
    }

    // 다른 콜라이더와 물리적으로 부딪히기 시작할 때 유니티가 자동 호출
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌한 대상의 태그가 "Ground"라면 바닥에 닿은 것으로 판단
        if (collision.gameObject.CompareTag("Ground") == true)
        {
            isGrounded = true;
        }
    }

    // 다른 콜라이더와 떨어질 때 유니티가 자동 호출
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 충돌하던 대상("Ground")에서 떨어지면 공중에 떠 있는 상태로 판단
        if (collision.gameObject.CompareTag("Ground") == true)
        {
            isGrounded = false;
        }
    }

    // 입력 방향에 따라 캐릭터의 이미지를 반전시키는 함수
    void UpdateDirection()
    {
        // 현재 게임 오브젝트의 Scale(크기) 정보를 가져옴
        Vector3 scale = transform.localScale;

        // 오른쪽 방향(+값) 입력 시 Scale X를 1로 (정방향)
        if (moveInput > 0.0f)
        {
            scale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        // 왼쪽 방향(-값) 입력 시 Scale X를 -1로 (좌우 반전)
        else if (moveInput < 0.0f)
        {
            scale = new Vector3(-1.0f, 1.0f, 1.0f);
        }

        // 계산된 Scale 값을 실제 오브젝트에 다시 적용
        transform.localScale = scale;
    }

    // 애니메이터의 파라미터를 갱신하여 애니메이션을 전환하는 함수
    void UpdateAnimation()
    {
        // 애니메이터가 할당되지 않았다면 함수 종료
        if (animator == null)
        {
            return;
        }

        bool move = false;

        // 입력값이 0이 아니라면 움직이고 있는 상태로 판단
        if (moveInput != 0.0f)
        {
            move = true;
        }

        // 애니메이터 컨트롤러에 정의된 "Move"와 "isGrounded" 변수에 값 전달
        animator.SetBool("Move", move);
        animator.SetBool("isGrounded", isGrounded);
    }
}