using UnityEngine;

public class EnemyPatrolsensor : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 2.0f;

    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private float wallCheckDistance = 0.3f;

    [SerializeField]
    private float groundCheckDistance = 0.6f;

    [SerializeField]
    private LayerMask groundLayerMask; // 광선이 해당 레이어로 지정된 오브젝트만 체크하도록.

    [SerializeField]
    private bool flipByScaleX = true;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    public Animator animator;

    private int moveDirectionSign = 1; // 현재 방향 지정(-1 왼쪽, +1 오른쪽)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveDirectionSign = 1;

        ApplyFacing();
    }

    private void FixedUpdate()
    {
        Move();

        bool hitwall = CheckWall(); // 방해물 체크
        bool hitCliff = CheckCliff(); // 낭떠러지 체크

        if (hitwall == true || hitCliff == true)
        {
            Turn();
        }

    }

    void Move()
    {
        Vector2 velocity = rb.linearVelocity;
        velocity.x = moveSpeed * moveDirectionSign;
        rb.linearVelocity = velocity;

        if (animator != null)
        {
            animator.SetBool("move", true);
        }
    }


    bool CheckWall()
    {
        // 광선을 쏠 방향 계산
        // + 면 오른쪽, - 면 왼쪽
        Vector2 dir = Vector2.right * moveDirectionSign;
        RaycastHit2D hit = Physics2D.Raycast(wallCheck.position, dir, wallCheckDistance, groundLayerMask);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 전방에 낭떠러지가 있는지 체크한다.
    /// </summary>
    bool CheckCliff()
    {
        //RaycastHit2D Physics2D.Raycast
        // - 지정한 위치에서 지정한 방향으로 지정한 길이 만큼 보이지 않는 광선을 쏴서 
        // 광선에 명중한 대상의 정보를 반환해주는 함수.
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position,Vector2.down, groundCheckDistance, groundLayerMask);

        // 광선에 명중한 대상이 없어야 낭떠러지가 있는 것이다.
        if(hit.collider == null)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 이동 방향을 반대로 바꾼다.
    /// </summary>
    void Turn()
    {
        moveDirectionSign = -moveDirectionSign;
        ApplyFacing();
    }

    /// <summary>
    /// 현재 방향에 맞게 시각적 좌우 반전.
    /// </summary>
    void ApplyFacing()
    {
        if(flipByScaleX == false)
        {
            return; 
        }

        Vector3 s = transform.localScale; 

        if (moveDirectionSign > 0) // 현재 방향이 오른쪽이면
        {
            s.x = 1.0f;
        }
        else
        {
            s.x = -1.0f;
        }

        transform.localScale = s; 
    }
}
