using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
}
