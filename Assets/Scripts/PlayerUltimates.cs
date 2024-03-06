using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerUltimates : MonoBehaviour
{
    public float UltimateCooldown;
    public bool StartOnCooldown;

    public GameObject RadialFlareBullet;
    public float RadialFlareBulletForce;
    public int RadialFlareBulletCount;
    public float RadialFlareMaxRotations;
    public float RadialFlareDuration;

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


    void Start()
    {
        LastUltimateCastTime = Time.time;
        if (!StartOnCooldown)
        {
            LastUltimateCastTime -= UltimateCooldown;
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
            var bulletVelocity = bulletDirection * Vector2.up;
            var bulletInst = Instantiate(RadialFlareBullet, transform.position, bulletDirection);
            bulletInst.GetComponent<Rigidbody2D>().velocity = bulletVelocity * RadialFlareBulletForce;
            yield return new WaitForSeconds(delayPerBullet);
        }
    }

}
