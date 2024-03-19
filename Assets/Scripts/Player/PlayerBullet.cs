using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(BulletDamage);
        }
        Destroy(gameObject);
    }
}
