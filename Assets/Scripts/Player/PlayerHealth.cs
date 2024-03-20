using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class PlayerHealth : MonoBehaviour
{

    public Slider healthSlider;
    public float maxHealth = 100f;

    public float currentHealth { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }

    public static PlayerHealth Instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var healthBarVal = currentHealth * 100f / maxHealth;
        if (healthSlider.value != healthBarVal) 
        {
            healthSlider.value = healthBarVal;
        }
        if (currentHealth <= 0) 
        {
            SceneManager.LoadScene("GameOver");
        }
    }


    public void TakeDamage(float damageAmount) 
    {
        currentHealth -= damageAmount;
        StartCoroutine(DamageFlash());
    }


    IEnumerator DamageFlash() 
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}
