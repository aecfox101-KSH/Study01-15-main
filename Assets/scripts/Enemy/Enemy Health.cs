using NUnit;
using UnityEngine;

/// <summary>
/// 적 캐릭터의 체력을 관리.
/// 최대 체력과 현재 체력을 보관.
/// 외부에서 공격을 받으면 대미지 값을 받아 체력을 감소.
/// 체력이 0 이하가 되면 사망처리.
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    public int MaxHealth = 5; // 최대 체력값

    public int currentHealth = 0; // 현재 체력값 

    private EnemyInvincibility Invincibility;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = MaxHealth; // 시작했을 시의 체력을 100으로 만들어 주는 작업

        Invincibility = GetComponent<EnemyInvincibility>();
    }

    public void TakeDamage(int damage) // 데미지를 주는 함수
    {
        if (Invincibility != null && Invincibility.IsInvincible() == true)
        {
            return;
        }

        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die(); // die 함수 호출
        }

        else
        {
            if(Invincibility != null)
            {
                Invincibility.StartInvincibility();
            }
        }

    }

    void Die() // 죽었을 시의 함수 처리
    {
        Debug.Log("몬스터 사망");
        Destroy(gameObject); // 오브젝트 파괴 하는 식
    }

}
