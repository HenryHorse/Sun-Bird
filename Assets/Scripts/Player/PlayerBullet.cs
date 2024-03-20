using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float BulletLifetime;
    public float BulletDamage;
    public float ExplosionRadius = 3f;
    public float ExplosionDamage = 3f;
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
            var enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(BulletDamage);
            if (explosion != null)
            {
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);
                    if(distance < ExplosionRadius)
                    {
                        enemy.gameObject.GetComponent<EnemyHealth>().TakeDamage(ExplosionDamage);
                    }
                }
                Instantiate(explosion, collision.transform.position, Quaternion.identity);
            }
            
        }
        Destroy(gameObject);
    }
}
