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

    public enum Attacks
    {
        None,
        Spawn,
        Slice,
        Lunge,
    }

    public float DescentTime;

    public float LungeWindupTime;
    public float LungeAttackTime;
    public float LungeReturnTime;

    public GameObject EnemyBasic;
    public GameObject EnemyFast;
    public GameObject EnemyRanged;
    public GameObject EnemyTanky;

    public GameObject SliceAttackPrefab;

    public AudioClip DashSound;

    public GameObject DescentTarget { get; set; }
    public GameManager Spawner { get; set; }

    public DraculaPhase CurrentPhase { get; private set; }
    public Attacks CurrentAttack { get; private set; }

    public EnemyHealth Health { get; private set; }
    public AudioSource Audio { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Health = GetComponent<EnemyHealth>();
        Audio = GetComponent<AudioSource>();
        StartCoroutine(Descend());
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentPhase == DraculaPhase.Attacking && CurrentAttack == Attacks.None)
        {
            PerformRandomAttack();
        }
    }

    public void PerformRandomAttack()
    {
        CurrentAttack = (Attacks)Random.Range(1, 4);

        var attackCoroutine = CurrentAttack switch
        {
            Attacks.Spawn => SpawnEnemies(),
            Attacks.Slice => SliceAttack(),
            Attacks.Lunge => LungeAttack(),
            _ => NullAttack(),
        };

        StartCoroutine(attackCoroutine);
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

    public IEnumerator NullAttack()
    {
        CurrentAttack = Attacks.None;
        yield return null;
    }

    public IEnumerator SpawnEnemies()
    {
        var possibleSpawns = new GameManager.SpawnGroup[] {
            new(EnemyBasic, 10, 3f, 0f),
            new(EnemyRanged, 10, 3f, 0f),
            new(EnemyTanky, 5, 3f, 0f),
            new(EnemyFast, 10, 3f, 0f),
        };

        int index = Random.Range(0, possibleSpawns.Length);
        yield return Spawner.SpawnEnemies(possibleSpawns[index]);
        CurrentAttack = Attacks.None;
        yield return null;
    }

    public IEnumerator SliceAttack()
    {
        Vector3 slicePos = Random.Range(0, 2) == 0 ? new(-10, 0, 0) : new(10, 0, 0);
        Instantiate(SliceAttackPrefab, slicePos, Quaternion.identity);
        yield return new WaitForSeconds(3);
        CurrentAttack = Attacks.None;
        yield return null;
    }

    public IEnumerator LungeAttack()
    {
        var originalPos = transform.position;
        var targetPos = PlayerController.Instance.transform.position;
        var normalizedDisp = (targetPos - transform.position).normalized;
        var backupPos = originalPos - normalizedDisp * 2;

        var startTime = Time.time;
        while (Time.time < startTime + LungeWindupTime)
        {
            var t = 1f - (startTime + LungeWindupTime - Time.time) / LungeWindupTime;
            transform.position = Vector3.Lerp(originalPos, backupPos, t);
            yield return null;
        }

        Audio.PlayOneShot(DashSound);

        startTime = Time.time;
        while (Time.time < startTime + LungeAttackTime)
        {
            var t = 1f - (startTime + LungeAttackTime - Time.time) / LungeAttackTime;
            transform.position = Vector3.Lerp(backupPos, targetPos, t);
            yield return null;
        }

        startTime = Time.time;
        while (Time.time < startTime + LungeReturnTime)
        {
            var t = 1f - (startTime + LungeReturnTime - Time.time) / LungeReturnTime;
            transform.position = Vector3.Lerp(targetPos, originalPos, t);
            yield return null;
        }

        CurrentAttack = Attacks.None;
        yield return null;
    }


}
