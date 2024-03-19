using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float DamageOnHit;
    public bool DestroyedOnHit;
    public float HitCooldown;

    public EnemyHealth HealthController { get; private set; }

    public bool IsTouchingPlayer { get; private set; }
    public float LastHitTime { get; private set; }



    // Start is called before the first frame update
    void Start()
    {
        HealthController = GetComponent<EnemyHealth>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsTouchingPlayer)
        {
            if (Time.fixedTime >= LastHitTime + HitCooldown)
            {
                LastHitTime = Time.fixedTime;
                PlayerHealth.Instance.TakeDamage(DamageOnHit);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var other = collision.gameObject.GetComponent<PlayerHealth>();
            if (DestroyedOnHit)
            {
                other.TakeDamage(DamageOnHit);
                Destroy(gameObject);
                return;
            }
            IsTouchingPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsTouchingPlayer = false;
        }
    }
}
