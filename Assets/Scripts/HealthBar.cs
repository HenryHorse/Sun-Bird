using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class HealthBar : MonoBehaviour
{

    public Slider healthSlider;
    public float maxHealth = 100f;
    public float currentHealth;

    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (healthSlider.value != currentHealth) 
        {
            healthSlider.value = currentHealth;
        }
        if (currentHealth <= 0) 
        {
            SceneManager.LoadScene(1);
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
