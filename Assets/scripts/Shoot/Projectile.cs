using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 8.0f; // 총알 속도

    [SerializeField]
    private int damage = 1; // 총알 데미지

    [SerializeField]
    private float lifeTime = 3.0f; // 총알 수명(총알이 발사 후 사라지기까지의 시간)

    private float lifeTimer = 0.0f; // 총알 수명 타이머
    private Vector2 direction = Vector2.right; // 총알 이동 방향

    private ProjectilePool pool = null; // 총알 풀 스크립트 참조

    /// <summary>
    /// 재활용을 위해 활성화 시 타이머 초기화.
    /// </summary>
    private void OnEnable()
    {
        lifeTimer = 0.0f;
    }

    public void SetPool(ProjectilePool projectilePool)
    {
        pool = projectilePool;
    }

    public void SetDirection(Vector2 dir)
    {
        /*순수한 방향 정보만 가지고 있는 벡터는 크기가 1이어야한다. 
        그래서 크기가 1이 아닌 벡터를 방향 정보로 사용하고자 할 경우
        이 벡터의 크기를 1로 만들어 줘야한다.
        그렇지 않을 경우 우리가 의도한 속도보다 더 빠르게 날아갈수 있다.*/
        // 벡터의 정규화 = 벡터의 크기를 1로 만들어 주는 것
        // 벡터의 크기가 1인 백터를 단위 벡터(Unit Vector)라 함. 
        direction = dir.normalized; // normalized
    }

    // 매 프레임마다 벡터의 값을 이동하는 함수
    void Move()
    {
        // 이동량 계산. = 방향 * 속력
        Vector2 delta = direction * moveSpeed * Time.deltaTime;

        // 현재 위치 저장.
        Vector3 pos = transform.position;

        // 새로 이동할 위치 계산.
        float newX = pos.x + delta.x;
        float newY = pos.y + delta.y;

        // 오브젝터의 위치 갱신. : z좌표 값은 기존의 값 유지
        transform.position = new Vector3(newX, newY, pos.z);
    }

    // 자동 파괴 시간 처리
    void UpdateLifetime()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            // Destroy(gameObject);
            ReturnToPool();
        }
    }

    // 적에게 총을 맞을 때

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 오브젝트가 적인지 확인
        if (collision.gameObject.CompareTag("Player") == true)
        {
            return;
        }
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>(); // 충돌한 적의 체력 스크립트 가져오기

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage); // 적에게 데미지 적용
        }

        PlayerKnockback playerKnockback = collision.gameObject.GetComponent<PlayerKnockback>();
        if (playerKnockback != null)
        {
            Vector2 knockbackDirection = direction; // 총알의 이동 방향을 넉백 방향으로 사용
            playerKnockback.ApplyKnockback(knockbackDirection); // 적에게 넉백 적용
        }

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioType.GainCoin);
        }

        if (ParticleManager.instance != null)
        {
            ParticleManager.instance.PlayFX(ParticleType.fire, transform.position);
        }

        // Destroy(gameObject); // 총알 파괴
        ReturnToPool();
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        UpdateLifetime();
    }
    /// <summary>
    /// 사용이 끝난 총알을 반환.
    /// </summary>
    void ReturnToPool()
    {
        if(pool != null)
        {
            pool.Return(this);
            return;
        }

        Destroy(gameObject);
    }
}
