using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // 인스펙터 창에는 노출이 되지만, 다른 클래스에서는 접근할수 없도록...
    [SerializeField]
    private int maxHealth = 5; // 최대 체력값

    private int currentHealth = 0; // 현재 체력값

    public GameObject gameOverUl;

    private bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth; // 시작했을 시의 체력을 100으로 만들어 주는 작업
    }

    public void TakeDamage(int damage) // 데미지를 주는 함수
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die(); // die 함수 호출
        }
    }

    void Die() // 죽었을 시의 함수 처리
    {
        Debug.Log("골까닥");
        isDead = true;

        if(gameOverUl != null)
        {
            gameOverUl.SetActive(true);
        }

        // gameObject.SetActive(false);
    }

    // 01.06
    public void Heal(int amount)
    {
        if(isDead == true)
        {
            return;
        }

        if (currentHealth == maxHealth)
        {
            return ;
        }

        currentHealth += amount;

        if(currentHealth >  maxHealth)
        {
            currentHealth = maxHealth;
        }
    }


    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    private bool IsDead()
    {
        return IsDead();
    }

}
