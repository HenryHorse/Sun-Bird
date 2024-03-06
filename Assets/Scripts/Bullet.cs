using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLifetime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, bulletLifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        if (PlayerController.Instance.ultimate)
        {
            Destroy(collision.gameObject);
        }
        
    }
}