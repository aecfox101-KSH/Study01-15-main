using UnityEngine;

/// <summary>
/// 플레이어의 기본 공격을 담당하는 클래스
/// 공격 키 입력을 감지.
/// 공격 쿨타임 관리.
/// 공격 애니메이션 재생.
/// </summary>
public class PlayerCombat : MonoBehaviour
{
    [Tooltip("공격 입력 키 지정 변수")] //인스펙터 창의 설명, [Header]
    public KeyCode attackKey = KeyCode.J; // 공격 입력 키 지정

    public float attackCooldown = 0.5f; // 다음 공격 가능 시간까지 도달하는 위한 쿨타임.

    public Animator animator;

    public Transform attackPoint; // 공격 중심 위치
    public float attackRange; // 공격이 닿는 반지름 거리
    public int attackDamage = 1; // 적에게 입힐 데미지 수치

    public LayerMask targetLayer; // 반드시 public 함수로 해야함

    private float attackTimer; // 공격 쿨타임 체크를 위한 타이머 변수.

    private bool isAttacking; // 현재 공격중인지 여부.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackTimer = 0.0f;
        isAttacking = false; // 초기화 작업
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAttackTimer();
        HeadleAttackInput();
    }

    /// <summary>
    /// 공격 쿨타임 타이머를 체크하기 위한 함수. 
    /// </summary>
    void UpdateAttackTimer()
    {
        if (attackTimer > 0.0f)
        {
            attackTimer -= Time.deltaTime; // 매 프레임 값을 뺌. 0보다 작보다 같은 값이 될시 공격이 가능하다는 상태 확인.
            if (attackTimer <= 0.0f)
            {
                attackTimer = 0.0f;
                isAttacking = false;
            }
        }

    }
    /// <summary>
    /// 공격 키 입력을 받아 공격 상태로 전환
    /// </summary>
    void HeadleAttackInput()
    {
        if(Input.GetKeyDown(attackKey)== true && attackTimer <= 0.0f)
        {
            StartAttack();
        }
    }
    /// <summary>
    /// 공격 시작.
    /// </summary>
    void StartAttack()
    {
        if(animator ==  null)
        {
            return;
        }

        animator.SetTrigger("Attack");

        attackTimer = attackCooldown; // 업데이트 함수에서 0이 될때까지 돌아감.
        isAttacking = true; // 공격 

        PerformAttackHit();
    }

    void PerformAttackHit() // 공격 중심점 설정
    {
        if (attackPoint == null)
        {
            attackPoint = transform;
        }

        Vector2 center = attackPoint.position;

        Collider2D[] hit = Physics2D.OverlapCircleAll(center, attackRange, targetLayer); // 범위 내 적 탐색 로직, center 위치에서 attackRange 반지름만큼의 가상의 원을 그려서, 그 안에 들어온 모든 **Collider2D(충돌체)**를 배열 형태로 가져오는 것
        // targetLayer 만 지정해주기 위함.

        for (int i = 0; i < hit.Length; ++i) // 데미지 입히는 반복문
        {
            EnemyHealth health=  hit[i].GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(attackDamage);
            }

            ApplyKnockback(hit[i]);
        }
    }

    void ApplyKnockback(Collider2D hit)
    {
        PlayerKnockback knockback = hit.gameObject.GetComponent<PlayerKnockback>();
        if (knockback != null)
        {
            Vector2 direction = Vector2.right;

            float diffX = hit.transform.position.x - transform.position.x;

            if (diffX < 0.0f)
            {
                direction = Vector2.left;
            }

            knockback.ApplyKnockback(direction);
        }
    }

    private void OnDrawGizmos() // 씬화면에서 우리가 원하는 라인, 사각형, 원, 구 같은 추가하려는 기능이 제대로 작동하는지 검사하는 함수.
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }

}
