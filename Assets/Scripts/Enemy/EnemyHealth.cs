using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float MaxHealth;

    public float CurrentHealth { get; private set; }
    public SpriteRenderer Sprite { get; private set; }

    public bool Invulnerable { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        Sprite = GetComponent<SpriteRenderer>();
        Invulnerable = false;
    }

    public void TakeDamage(float damage)
    {
        if (Invulnerable)
        {
            return;
        }
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            PlayerStats.Instance.EnemyKills++;
            Destroy(gameObject);
        }
        StartCoroutine(DamageFlash());
    }

    IEnumerator DamageFlash()
    {
        Sprite.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        Sprite.color = Color.white;
    }
}
