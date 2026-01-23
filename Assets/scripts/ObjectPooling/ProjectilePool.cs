using UnityEngine;
using System.Collections.Generic; // List<T> 사용을 위함

public class ProjectilePool : MonoBehaviour
{
    [SerializeField]
    private Projectile projectilePrefab; // 풀링할 총알 프리팹

    [SerializeField]
    private int initialSize = 30; // 초기 풀 크기

    [SerializeField]
    private bool canExpand = true; // 풀 확장 가능 여부

    [SerializeField]
    private int expandCount = 10; // 확장 시 추가할 총알의 수

    private List<Projectile> allProjectiles = new List<Projectile>(); // 모든 총알 리스트

    private void Awake() // 게임 오브젝트가 활성화 될 때 호출되는 함수
    {
        Prewarm(); // 게임 시작 시 총알 미리 생성
    }

    /// <summary>
    /// 게임 시작 시 총알 오브젝트 미리 생성하는 함수
    /// </summary>
    void Prewarm()
    {
        for (int i = 0; i < initialSize; ++i)
        {
            CreateProjectile();
        }
    }

    Projectile CreateProjectile()
    {
        GameObject go = Instantiate(projectilePrefab.gameObject,transform.position, Quaternion.identity);

        if(go == null)
        {
               return null;
        }

        Projectile projectile = go.GetComponent<Projectile>();
        if (projectile == null)
        {
            Destroy(go);
            return null;
        }

        // 일단 총알의 부모를 Pool 오브젝트로 설정
        go.transform.SetParent(transform); 

        // 투사체 스크립트에 소유자 정보를 남겨준다.
        // 조금 있다가 합시다.
        projectile.SetPool(this);

        projectile.gameObject.SetActive(false); // 비활성화 상태로 시작
        allProjectiles.Add(projectile); // 리스트에 추가

        return projectile;
    }

    public Projectile Get() // 총알을 꺼내는 함수
    {
        Projectile p = null;
        if (allProjectiles.Count > 0) // 리스트에 사용가능한 총알이 있을 경우.
        {
            p = allProjectiles[0]; // 리스트에서 첫 번째 총알을 가져옴
            p.gameObject.SetActive(true); // 가져온 총알 활성화
            allProjectiles.RemoveAt(0); // 가져온 총알을 리스트에서 삭제
        }
        else
        {
            // 리스트에 남은 총알이 없고 추가생성을 할 수 있다면,
            if (canExpand == true)
            {
                for (int i = 0; i < expandCount; ++i)
                {
                    CreateProjectile();
                }

                p = allProjectiles[0]; // 리스트에서 첫 번째 총알을 가져옴
                p.gameObject.SetActive(true); // 활성화
                allProjectiles.RemoveAt(0); // 리스트에서 제거
            }
        }
        return p;
    }

    public void Return(Projectile projectile) // 총알을 다시 반환하는 함수
    {
        projectile.gameObject.SetActive(false); // 총알 비활성화
        allProjectiles.Add(projectile); // 리스트에 다시 추가
    }
}
