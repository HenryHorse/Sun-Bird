using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class PlayerAbilities : MonoBehaviour
{
    public static PlayerAbilities Instance;
    public float UltimateCooldown;
    public bool StartOnCooldown;
    public Image UltimateCooldownImage;

    public GameObject RadialFlareBullet;
    public float RadialFlareBulletForce;
    public int RadialFlareBulletCount;
    public float RadialFlareMaxRotations;
    public float RadialFlareDuration;

    public int FiewWaveBulletCount;
    public float FireWavesCD;


    public float FireBombCD;
    public float searchRadius = 3f;
    public GameObject FireBomb;

    public float LastUltimateCastTime { get; private set; }
    public float UltimateCooldownTimeRemaining
    {
        get => LastUltimateCastTime + UltimateCooldown - Time.time;
        set => LastUltimateCastTime += value - UltimateCooldownTimeRemaining;
    }
    public bool IsUltimateAvailable
    {
        get => UltimateCooldownTimeRemaining <= 0;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    void Start()
    {
        LastUltimateCastTime = Time.time;
        if (!StartOnCooldown)
        {
            LastUltimateCastTime -= UltimateCooldown;
        }
    }

    private void Update()
    {
        if (UltimateCooldownImage != null)
        {
            if (IsUltimateAvailable)
            {
                UltimateCooldownImage.fillAmount = 0f;
            }
            else
            {
                UltimateCooldownImage.fillAmount = UltimateCooldownTimeRemaining / UltimateCooldown;
            }
        }
    }

    public IEnumerator CastRadialFlare()
    {
        if (!IsUltimateAvailable)
        {
            yield break;
        }
        LastUltimateCastTime = Time.time;
        var anglePerBullet = (RadialFlareMaxRotations * 360f / RadialFlareBulletCount);
        var delayPerBullet = RadialFlareDuration / RadialFlareBulletCount;
        for (int i = 0; i < RadialFlareBulletCount; i++)
        {
            var angle = anglePerBullet * i;
            var bulletDirection = Quaternion.Euler(new(0f, 0f, angle));
            var bulletVelocity = bulletDirection * Vector2.right;
            var bulletInst = Instantiate(RadialFlareBullet, transform.position, bulletDirection);
            bulletInst.GetComponent<Rigidbody2D>().velocity = bulletVelocity * RadialFlareBulletForce;
            yield return new WaitForSeconds(delayPerBullet);
        }
    }

    public IEnumerator CastFireWave(int level)
    {
        var currentCooldown = FireWavesCD / Mathf.Sqrt(level);
        while (true)
        {
            var anglePerBullet = 360f / FiewWaveBulletCount;
            for (int i = 0; i < FiewWaveBulletCount; i++)
            {
                var angle = anglePerBullet * i;
                var bulletDirection = Quaternion.Euler(new(0f, 0f, angle));
                var bulletVelocity = bulletDirection * Vector2.right;
                var bulletInst = Instantiate(RadialFlareBullet, transform.position, bulletDirection);
                bulletInst.GetComponent<Rigidbody2D>().velocity = bulletVelocity * RadialFlareBulletForce;
            }
            yield return new WaitForSeconds(currentCooldown);
        }
        
    }

    public IEnumerator CastFireBomb(int level)
    {
        var currentCooldown = FireBombCD / Mathf.Sqrt(level);    
        while (true)
        {
            int highestDensity = 0;
            GameObject targetEnemy = null;
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                int currentDensity = 1;
                Vector3 enemyPosition = enemy.transform.position;

                foreach (GameObject other in enemies)
                {
                    if (enemy == other) continue;

                    float distance = Vector3.Distance(enemyPosition, other.transform.position);
                    if(distance < searchRadius)
                    {
                        currentDensity++;
                    }
                }
                if(currentDensity > highestDensity)
                {
                    highestDensity = currentDensity;
                    targetEnemy = enemy;
                }
            }
           
            if (targetEnemy != null)
            {
                Vector3 direction = targetEnemy.transform.position - transform.position;
                float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, 0, rotZ);
                GameObject fireBomb = Instantiate(FireBomb, transform.position, rotation);
                fireBomb.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y).normalized * 10f;
            }
            yield return new WaitForSeconds(currentCooldown);
        }

    }
}
