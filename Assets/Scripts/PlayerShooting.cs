using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Camera MainCamera;
    public GameObject projectile;
    public Transform shotPoint;
    public float firePower;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 direction = (mousePosition - transform.position).normalized;
                FireProjectile(direction);
            }
        
    }

    public void FireProjectile(Vector3 direction) {
        GameObject projectileInstance = Instantiate(projectile, shotPoint.position, Quaternion.identity);
        projectileInstance.GetComponent<Rigidbody2D>().velocity = direction * firePower;
    }
}
