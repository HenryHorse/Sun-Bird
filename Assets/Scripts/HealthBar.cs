using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider healthSlider;
    public float maxHealth = 100f;
    public float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
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
}
