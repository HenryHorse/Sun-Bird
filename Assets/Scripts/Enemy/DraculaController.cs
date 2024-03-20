using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraculaController : MonoBehaviour
{
    public enum DraculaPhase
    {
        Descending,
        Attacking,
        Defeated,
    }

    public float DescentTime;

    public GameObject DescentTarget { get; set; }

    public DraculaPhase CurrentPhase { get; private set; }

    public EnemyHealth Health { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Health = GetComponent<EnemyHealth>();
        StartCoroutine(Descend());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Descend()
    {
        CurrentPhase = DraculaPhase.Descending;
        Health.Invulnerable = true;

        var initialPos = transform.position;
        var targetPos = DescentTarget.transform.position;

        var startTime = Time.time;
        while (Time.time < startTime + DescentTime)
        {
            var t = 1f - (startTime + DescentTime - Time.time) / DescentTime;
            transform.position = Vector3.Lerp(initialPos, targetPos, t);
            yield return null;
        }

        CurrentPhase = DraculaPhase.Attacking;
        Health.Invulnerable = false;
    }
}
