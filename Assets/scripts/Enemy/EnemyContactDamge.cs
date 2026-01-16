using UnityEngine;

public class EnemyContactDamge : MonoBehaviour
{
    public int Damage = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.tag == "Player") ;
        if(collision.gameObject.CompareTag("Player") == true)
        {
            PlayerInvincibility inv = collision.gameObject.GetComponent<PlayerInvincibility>();
            if(inv != null && inv.IsInvincible() == true)
            {
                return;
            }

            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth !=null)
            {
                playerHealth.TakeDamage(Damage);
                Debug.Log("¶¨");
            }

            if(inv !=null)
            {
                inv.StartInvincibility();
            }



            PlayerKnockback knockback = collision.gameObject.GetComponent<PlayerKnockback>();
            if (knockback != null)
            {
                Vector2 direction = Vector2.right;

                float diffX = collision.transform.position.x - transform.position.x;

                if(diffX < 0.0f)
                {
                    direction = Vector2.left;
                }

                knockback.ApplyKnockback(direction);
            }
        }
    }
}
