using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.name == "Player(Placeholder)"){
            Vector2 rand = new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
            transform.position = rand;
        }
    }
}
