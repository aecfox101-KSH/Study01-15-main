using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    // - 설정 변수들 -
    public Transform leftPoint; // 왼쪽 이동 한계점 (InSpector 설정)
    public Transform rightPoint; // 오른쪽 이동 한계점 (InSpector 설정)

    public float moveSpeed = 2.0f; // 적의 이동 속도

    public float pointPeachThreshold = 0.05f; // 목표 지점에서 도착했다 고 판단할 거리

    public bool moveRight = true; // 현재 오른쪽으로 가고 있는지 여부
    public SpriteRenderer spriteRenderer; // 캐릭터 이미지를 좌우 반전시키기 위한 컴포넌트

    public Animator animator;

    private EnemyKnockback enemyKnockback;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() // 게임이 시작될 때 한번 실행
    {
        moveRight = true; // 처음 시작할 때 이동 방향을 오른쪽으로 설정
        enemyKnockback = GetComponent<EnemyKnockback>();
    }

    // Update is called once per frame
    void Update()  // 매 프레임마다 실행
    {
        CheckTurnAround();  // 1. 방향을 바꿔야 하는지 먼저 확인
        Move(); // 2. 현재 방향으로 이동
    }
    //  -이동기능-
    void Move()
    {
        float directionX = 0.0f;

        // 방향에 따라 x축 이동 값을 설정
        if(moveRight == true)
        {
            directionX = 1.0f; // 오른쪽 양수(+)
        }
        else
        {
            directionX = -1.0f; // 왼쪽은 음수(-)
        }

        // 이동 거리 계산 = 속도 * 시간(프레임 보정) * 방향
        float distance = moveSpeed * Time.deltaTime * directionX;

        /*Vector3 currentPosition = transform.position;
        currentPosition += new Vector3(distance, 0.0f, 0.0f);
        transform.position = currentPosition;*/  // 1번

        /*transform.position += new Vector3(distance, 0.0f, 0.0f);*/ //2번,  1번 2번꺼랑 동일함.

        /// 현재 위치를 가져와 계산된 거리만큼 더해 새로운 위치 적용
        Vector3 currentposition = transform.position;
        Vector3 newPosition = currentposition + new Vector3 (distance, 0.0f, 0.0f); 
        transform.position = newPosition;

        if(animator != null )
        {
            animator.SetBool("move", true);
        }
    }

    // -반전 체크 기능-
    void CheckTurnAround()
    {
        // 목표 지점 오브젝트가 연결 안 되어 있으면 실행하지 않음.(에러방지)
        if (leftPoint == null || rightPoint == null)
        {
            return;
        }

        float currentX = transform.position.x; //나의 현재 X 위치
        float leftX= leftPoint.position.x; // 왼쪽 지점의  X 위치
        float rightX= rightPoint.position.x; // 오른쪽 지점의 X 위치


        // 오른쪽으로 가고 있을때
        if(moveRight == true)
        { 
            // 목표(오른쪽 끝)와의 거리를 계산 (Mathf.Abs는 항상 양수로 만듦)
            float  distanceToRight = Mathf.Abs(rightX - currentX); // 절대값으로 계산해주는 함수 Mathf.Abs(값)

            // 목표 지점에 충분히 가까워졌다면
            if (distanceToRight <= pointPeachThreshold)
            {
                //방향을 바꿔주는 처리
                TurnAround();  // 뒤돌아~
            }
        }

        // 왼쪽으로 가고 있을때
        else
        {
            // 목표(왼쪽 끝)와의 거리를 계산
            float distanceToLeft = Mathf.Abs(leftX - currentX); // 값의 값에 순서가 바꿔도 상관없음 why? 절대값으로 반환해주기 때문
            if(distanceToLeft <= pointPeachThreshold)
            {
                //방향을 바꿔주는 처리
                TurnAround();
            }
        }
    }
    // --- 실제로 방향을 바꾸는 기능 ---
    void TurnAround()
    {
        if (moveRight == true)
        {
            moveRight = false;
        }
        else if(moveRight == false)
        {
            moveRight = true;
        }

       //moveRight = !moveRight;
       //moveRight = (moveRight == false);

        if(spriteRenderer != null)
        {
            if(moveRight == true)
            {
                spriteRenderer.flipX = false; // 체크를 안하겠다고 하면 false
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
    }

}
