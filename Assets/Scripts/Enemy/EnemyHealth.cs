using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float MaxHealth;

    public float CurrentHealth { get; private set; }
    public SpriteRenderer Sprite { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        Sprite = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
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
