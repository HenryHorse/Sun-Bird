using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    private ParticleSystem partSystem;
    public float ExplosionDamage = 3f;

    void Start()
    {
        partSystem = GetComponent<ParticleSystem>();
        var collisionModule = partSystem.collision;
        collisionModule.sendCollisionMessages = true;
    }

    // This function is called when a particle collides with a GameObject
    void OnParticleCollision(GameObject other)
    {

        if (other.CompareTag("Enemy")) 
        {
            var enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(ExplosionDamage);
        }
    }
}