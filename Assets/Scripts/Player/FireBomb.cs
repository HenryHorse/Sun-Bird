using System.Collections;
using UnityEngine;

public class FireBomb : MonoBehaviour
{
    public float BulletLifetime;
    public float BulletDamage;
    public float explodeRadius = 3f;
    [SerializeField] ParticleSystem explosion;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, BulletLifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(explosion, collision.transform.position, Quaternion.identity);
            var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(BulletDamage);
            
        }
        Destroy(gameObject);
    }
}