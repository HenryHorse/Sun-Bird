using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Rigidbody2D Projectile;
    public float ShootingRange;
    public float ShootingCooldown;
    public float ShootingSpeed;

    public GameObject Player { get; private set; }
    public float LastShotTime { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<AIChase>().Player;
        LastShotTime = Time.fixedTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var displacement = Player.transform.position - transform.position;
        if (Time.fixedTime > LastShotTime + ShootingCooldown &&
            displacement.sqrMagnitude <= ShootingRange * ShootingRange)
        {
            LastShotTime = Time.fixedTime;
            var angleFloat = Vector2.Angle(Vector2.right, displacement) * (displacement.y > 0 ? -1 : 1);
            var angle = Quaternion.AngleAxis(angleFloat, Vector3.back);
            var projObj = Instantiate(Projectile, transform.position, angle);
            projObj.velocity = angle * Vector2.right * ShootingSpeed;
        }
    }
}
