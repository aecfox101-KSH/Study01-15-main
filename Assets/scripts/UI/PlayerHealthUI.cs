using UnityEngine;
using UnityEngine.UI; // namespace (이름 공간) 

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image imageHealth;
    [SerializeField] private PlayerHealth playerHealth; // 플레이의 HP 스크립트 불러오는 코드

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth == null) 
        {
            return;
        }

        // 매 프레임마다 플레이어의 현재 HP와 최대 HP 정보를 가져와서 UI를 갱신하는 코드를 작성.
        int currentHealth = playerHealth.GetCurrentHealth(); //현재 HP 정보 갖고옴
        int maxHealth = playerHealth.GetMaxHealth(); // 최대 HP 정보
        
       if (currentHealth <=0)
        {
            imageHealth.fillAmount = 0; 
            return;
        }
        
        float health = (float)currentHealth / (float)maxHealth; // 현재 hp 나누기 최대 hp
        imageHealth.fillAmount = health;

        /*
        if (playerHealth.GetCurrentHealth() <= 0)
        {
            imageHealth.fillAmount = 0;
            return;
        }

        imageHealth.fillAmount = (float)playerHealth.GetCurrentHealth() / (float)playerHealth.GetMaxHealth();
        */
    }
}
