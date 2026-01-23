using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField]
    private Projectile projectilePrefab;

    [SerializeField]
    private Transform firePoint; // 총알 발사 위치 변수 

    [SerializeField]
    private float fireCooldown = 0.5f; // 발사 시간 값

    [SerializeField]
    private KeyCode fireKey = KeyCode.F; // 발사 키 지정

    private float nextFireTime = 0.0f;// 발사 쿨타임 타이머

    [SerializeField]
    private ProjectilePool projectilePool; // 총알 풀 스크립트 참조 변수

    void Fire() // 총알을 발사할 함수
    {
        if (projectilePrefab == null)// 총알 프리팹이 할당되어 있지 않으면 발사하지 않음
        {
            return;
        }

        if (firePoint == null)
        {
            return; 
        }

        Projectile projectile = projectilePool.Get();
        if (projectile == null)
        {
            return;
        }
        projectile.transform.position = firePoint.position;

        // 캐릭터의 방향을 알아오기 위해서 scale.x 값을 가져온다.
        float scaleX = transform.localScale.x;

        // 기본 방향을 오른쪽으로 설정.
        Vector2 dir = Vector2.right;

        // 캐릭터가 왼쪽을 향하고 있는지 검사
        if (scaleX < 0.0f)
        {
            dir = Vector2.left;
        }
        // 총알의 이동 방향 설정.
        projectile.SetDirection(dir);
    }

    /// <summary>
    /// 발사 키 입력 처리.
    /// </summary>
    void HandleFireInput()
    {
        if(Input.GetKeyDown(fireKey) == false)
        {
            return;
        }

        if(Time.time < nextFireTime)
        {
            return;
        }

        nextFireTime = Time.time + fireCooldown;

        Fire();

    }

    // Update is called once per frame
    void Update()
    {
        HandleFireInput();
    }
}
