using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeSystem : MonoBehaviour
{
    public int nextUpgradeThreshold = 3;
    public GameObject upgradeMenu;
    public TextMeshProUGUI upgradePrompt;
    public Dictionary<string, int> abilities = new Dictionary<string, int>();
    private Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();

    private void Start()
    {
        upgradePrompt.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        var enemyKills = PlayerStats.Instance.EnemyKills;
        if (enemyKills >= nextUpgradeThreshold)
        {
            Upgrade();
            nextUpgradeThreshold += (enemyKills / 3 + 3);
        }
        upgradePrompt.text = "Next Upgrade: " + enemyKills + "/" + nextUpgradeThreshold;
    }

    void Upgrade()
    {
        Time.timeScale = 0;
        upgradeMenu.SetActive(true);
    }

    public void IncreaseBullets()
    {
        if (!abilities.ContainsKey("MoreBullets"))
        {
            abilities.Add("MoreBullets", 1);
        }
        else
        {
            abilities["MoreBullets"] += 1;
        }
        PlayerController.Instance.bulletsPerTap++;
        upgradeMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void DecreaseShootingTime()
    {
        if (!abilities.ContainsKey("SpeedUp"))
        {
            abilities.Add("SpeedUp", 1);
        }
        else
        {
            abilities["SpeedUp"] += 1;
        }
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

    public void FireBomb()
    {
        if (!abilities.ContainsKey("FireBomb"))
        {
            abilities.Add("FireBomb", 1);
            coroutines.Add("FireBomb", StartCoroutine(PlayerAbilities.Instance.CastFireBomb(abilities["FireBomb"])));
        }
        else
        {
            abilities["FireBomb"] += 1;
            StopCoroutine(coroutines["FireBomb"]);
            coroutines.Remove("FireBomb");
            coroutines.Add("FireBomb", StartCoroutine(PlayerAbilities.Instance.CastFireBomb(abilities["FireBomb"])));
        }
        upgradeMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
