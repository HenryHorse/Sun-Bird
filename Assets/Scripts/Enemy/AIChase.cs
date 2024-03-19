using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public GameObject Player;
    public float Speed;
    public float MinDistance;

    // Update is called once per frame
    void FixedUpdate()
    {
        var targetPos = Player.transform.position;
        var direction = (transform.position - targetPos).normalized;
        var targetDisplacement = direction * MinDistance;
        transform.position = Vector2.MoveTowards(transform.position, targetPos + targetDisplacement, Speed * Time.fixedDeltaTime);
    }
}
