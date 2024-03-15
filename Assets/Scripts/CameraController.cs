using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Target;
    public Vector2 TargetScale;

    public Camera Cam { get; private set; }

    private void Start()
    {
        Cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var w = TargetScale.x;
        var h = TargetScale.y;

        Cam.transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, -10f);

        if (w > h * Cam.aspect)
        {
            Cam.orthographicSize = ((float)w / Cam.pixelWidth * Cam.pixelHeight) / 2;
        }
        else
        {
            Cam.orthographicSize = h / 2;
        }
    }
}
