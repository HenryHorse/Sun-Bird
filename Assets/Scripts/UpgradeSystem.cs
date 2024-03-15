using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance;
    public int upgradeNumber = 3;
    public int enemyKills;
    public GameObject upgradeMenu;
    public TextMeshProUGUI upgradePrompt;
    public Dictionary<string, int> abilities = new Dictionary<string, int>();
    private Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }

    private void Start()
    {
        upgradePrompt.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyKills >= upgradeNumber)
        {
            Upgrade();
            upgradeNumber += (enemyKills / 3 + 3);
        }
        upgradePrompt.text = "Next Upgrade: " + enemyKills + "/" + upgradeNumber;
    }

    void Upgrade()
    {
        Time.timeScale = 0;
        upgradeMenu.SetActive(true);

    }

    public void IncreaseBullets()
    {
        PlayerController.Instance.bulletsPerTap++;
        upgradeMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void DecreaseShootingTime()
    {
        PlayerController.Instance.timeBetweenShooting /= 2;
        upgradeMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void FireWave()
    {
        if (!abilities.ContainsKey("FireWave")){
            abilities.Add("FireWave", 1);
            coroutines.Add("FireWave", StartCoroutine(PlayerAbilities.Instance.CastFireWave(abilities["FireWave"])));
        }
        else
        {
            abilities["FireWave"] += 1;
            StopCoroutine(coroutines["FireWave"]);
            coroutines.Remove("FireWave");
            coroutines.Add("FireWave", StartCoroutine(PlayerAbilities.Instance.CastFireWave(abilities["FireWave"])));
        }
        upgradeMenu.SetActive(false);
        Time.timeScale = 1;

    }
}
