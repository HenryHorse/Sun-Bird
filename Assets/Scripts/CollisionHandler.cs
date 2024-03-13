using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private HealthBar playerHealthBar;
    public float damageAmount = 20f;
    public float enemyHealth = 20f;

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) 
        {
            playerHealthBar = other.gameObject.GetComponent<HealthBar>();
            playerHealthBar.currentHealth -= damageAmount;
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            // Vector2 rand = new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
            // transform.position = rand;
            enemyHealth -= 5f;
            if (enemyHealth <= 0) {
                 Destroy(gameObject);
            }
        }
    }
}
