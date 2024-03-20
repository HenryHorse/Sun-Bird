using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

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

    public enum GameStatus
    {
        Stopped,
        InNormalWave,
        InBossFight,
    }


    public static GameManager Instance;


    public float SpawnRadius;
    public TextMeshProUGUI RoundText;
    public Camera Camera;
    public DraculaController DraculaPrefab;
    public GameObject DraculaDescentTarget;

    public GameStatus CurrentStatus { get; private set; }
    public int CurrentScore { get; set; }
    public int CurrentWaveIndex { get; private set; }
    public List<SpawnGroup> CurrentWave { get; private set; }
    public float CurrentWaveTime { get; private set; }
    public List<GameObject> Enemies { get; private set; }

    private List<Coroutine> RunningSpawners;
    private WaveInfo WaveInfo;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        CurrentStatus = GameStatus.Stopped;
        CurrentScore = 0;
        CurrentWaveTime = 0f;
        CurrentWaveIndex = 0;
        CurrentWave = new();
        Enemies = new();
        RunningSpawners = new();
        WaveInfo = GetComponent<WaveInfo>();

        MusicController.Instance.PlayWaveMusic();
        StartWave(CurrentWaveIndex);
        // StartCoroutine(StartBossRound());
    }

    // Update is called once per frame
    void Update()
    {
        Enemies.RemoveAll(obj => obj.IsDestroyed());
        if (CurrentStatus == GameStatus.InNormalWave)
        {
            UpdateWaveSpawns();
            StartNextWaveIfFinished();
        }
        else if (CurrentStatus == GameStatus.InBossFight)
        {
            if (Enemies.Count == 0)
            {
                StartCoroutine(EndBossRound());
            }
        }
    }

    private void UpdateWaveSpawns()
    {
        CurrentWaveTime += Time.deltaTime;

        var waveInProgress = false;
        foreach (var spawnGroup in CurrentWave)
        {
            if (spawnGroup.SpawnProgress == SpawnGroup.Progress.NONE)
            {
                waveInProgress = true;
                if (CurrentWaveTime > spawnGroup.Time)
                {
                    RunningSpawners.Add(StartCoroutine(SpawnEnemies(spawnGroup)));
                }
            }
            else if (spawnGroup.SpawnProgress == SpawnGroup.Progress.IN_PROGRESS)
            {
                waveInProgress = true;
            }
        }

        // check if enemies are on screen
        if (Enemies.Count > 0)
        {
            waveInProgress = true;
        }

        if (!waveInProgress)
        {
            CurrentStatus = GameStatus.Stopped;
        }
    }

    public void StartNextWaveIfFinished()
    {
        if (CurrentStatus == GameStatus.Stopped)
        {
            RunningSpawners.Clear();
            if (CurrentWaveIndex < WaveInfo.Waves.Length - 1)
            {
                CurrentWaveIndex++;
                StartWave(CurrentWaveIndex);
            }
            else
            {
                StartCoroutine(StartBossRound());
            }
        }
    }

    public void StartWave(int waveIndex)
    {
        RoundText.text = $"Round: {waveIndex + 1}";
        StartWave(WaveInfo.Waves[waveIndex]);
    }

    public void StartWave(List<SpawnGroup> wave)
    {
        CurrentStatus = GameStatus.InNormalWave;
        CurrentWave = wave;
        CurrentWaveTime = 0f;
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
            var newEnemy = Instantiate(spawnGroup.Enemy, transform, true);
            newEnemy.transform.position = enemyPosition;
            newEnemy.GetComponent<AIChase>().Player = PlayerController.Instance.gameObject;
            Enemies.Add(newEnemy);
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

    private IEnumerator StartBossRound()
    {
        RoundText.text = $"Round: ???";
        MusicController.Instance.PlayBossMusic();
        yield return new WaitForSeconds(12f);
        var dracula = Instantiate(DraculaPrefab, transform, true);
        var startY = Camera.ViewportToWorldPoint(new(1, 1, Camera.nearClipPlane)).y + 2f;
        dracula.transform.position = new Vector3(0, startY, dracula.transform.position.z);
        dracula.DescentTarget = DraculaDescentTarget;
        dracula.Spawner = this;
        Enemies.Add(dracula.gameObject);
        yield return new WaitForSeconds(1.5f);
        RoundText.text = $"Round: BOSS";
    }

    private IEnumerator EndBossRound()
    {
        foreach (var enemy in Enemies)
        {
            Destroy(enemy);
        }
        Debug.Log("yay");
        yield return null;
    }
}
