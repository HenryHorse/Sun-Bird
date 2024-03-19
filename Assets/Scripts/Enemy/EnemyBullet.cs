using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float BulletLifetime;
    public float BulletDamage;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, BulletLifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(BulletDamage);
        }
        Destroy(gameObject);
    }
}
