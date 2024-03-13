using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradeSystem : MonoBehaviour
{
    public float upgradeTime = 60f;
    public GameObject upgradeMenu;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Upgrade());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Upgrade()
    {
        while (true)
        {
            yield return new WaitForSeconds(upgradeTime);
            Time.timeScale = 0;
            upgradeMenu.SetActive(true);
        }

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
}
