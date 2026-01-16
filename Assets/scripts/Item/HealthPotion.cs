using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private int healthAmount = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") == false)
        {
            return;
        }

        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>(); // gameObject 생략 가능
        if (playerHealth != null)
        {
            playerHealth.Heal(healthAmount);
        }

        Destroy(gameObject);

    }

    

}
