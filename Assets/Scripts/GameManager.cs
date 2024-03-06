using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public class SpawnGroup
    {
        public enum Progress
        {
            NONE,
            IN_PROGRESS,
            DONE,
        }

        public GameObject Enemy { get; set; }
        public int Count { get; set; }
        public float Duration { get; set; }
        public float Time { get; set; }
        public Progress SpawnProgress { get; set; }

        public SpawnGroup(GameObject enemy, int count, float duration, float time)
        {
            Enemy = enemy;
            Count = count;
            Duration = duration;
            Time = time;
            SpawnProgress = Progress.NONE;
        }
    }

    public float SpawnRadius;

    public int CurrentScore { get; set; }
    public int CurrentWaveIndex { get; private set; }
    public List<SpawnGroup> CurrentWave { get; private set; }
    public float CurrentWaveTime { get; private set; }
    public bool WaveInProgress { get; private set; }

    private List<Coroutine> RunningSpawners;
    private WaveInfo WaveInfo;


    // Start is called before the first frame update
    void Start()
    {
        CurrentScore = 0;
        CurrentWaveTime = 0f;
        CurrentWaveIndex = 0;
        CurrentWave = new();
        WaveInProgress = false;
        RunningSpawners = new();
        WaveInfo = GetComponent<WaveInfo>();

        // temporary wave start
        StartWave(CurrentWaveIndex);
    }

    // Update is called once per frame
    void Update()
    {
        CurrentWaveTime += Time.deltaTime;

        WaveInProgress = false;
        foreach (var spawnGroup in CurrentWave)
        {
            if (spawnGroup.SpawnProgress == SpawnGroup.Progress.NONE)
            {
                WaveInProgress = true;
                if (CurrentWaveTime > spawnGroup.Time)
                {
                    RunningSpawners.Add(StartCoroutine(SpawnEnemies(spawnGroup)));
                }
            }
            else if (spawnGroup.SpawnProgress == SpawnGroup.Progress.IN_PROGRESS)
            {
                WaveInProgress = true;
            }
        }
    }

    public void StartWave(int waveIndex)
    {
        StartWave(WaveInfo.Waves[waveIndex]);
    }

    public void StartWave(List<SpawnGroup> wave)
    {
        CurrentWave = wave;
        CurrentWaveTime = 0f;
        WaveInProgress = true;
        foreach (var spawner in RunningSpawners)
        {
            StopCoroutine(spawner);
        }
        RunningSpawners.Clear();
    }

    public IEnumerator SpawnEnemies(SpawnGroup spawnGroup)
    {
        spawnGroup.SpawnProgress = SpawnGroup.Progress.IN_PROGRESS;
        var betweenEnemies = spawnGroup.Duration / spawnGroup.Count;
        for (int i = 0; i < spawnGroup.Count; i++)
        {
            var randomAngle = Random.Range(0f, 360f);
            var enemyPosition = Quaternion.Euler(new(0f, 0f, randomAngle)) * Vector2.up * SpawnRadius;
            var newEnemy = Instantiate(spawnGroup.Enemy, enemyPosition, Quaternion.identity);
            newEnemy.GetComponent<AIChase>().player = PlayerController.Instance.gameObject;
            if (i < spawnGroup.Count - 1)
            {
                yield return new WaitForSeconds(betweenEnemies);
            }
            else
            {
                spawnGroup.SpawnProgress = SpawnGroup.Progress.DONE;
            }
        }
    }
}
