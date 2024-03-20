using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveInfo : MonoBehaviour
{
    public GameObject BasicEnemy;
    public GameObject RangedEnemy;
    public GameObject TankyEnemy;


    public List<GameManager.SpawnGroup>[] Waves;


    void Awake()
    {
        Waves = new List<GameManager.SpawnGroup>[] {
            new(new GameManager.SpawnGroup[] {
                new(BasicEnemy, 5, 20f, 0f),
                new(BasicEnemy, 10, 20f, 20f),
                new(BasicEnemy, 10, 5f, 40f),
            }),
            new(new GameManager.SpawnGroup[] {
                new(BasicEnemy, 10, 20f, 0f),
                new(BasicEnemy, 10, 20f, 20f),
                new(RangedEnemy, 10, 20f, 20f),
                new(BasicEnemy, 20, 5f, 40f),
            }),
            new(new GameManager.SpawnGroup[] {
                new(TankyEnemy, 7, 40f, 0f),
                new(BasicEnemy, 20, 20f, 0f),
                new(BasicEnemy, 20, 20f, 20f),
                new(RangedEnemy, 20, 20f, 20f),
                new(BasicEnemy, 40, 5f, 40f),
            }),
        };
    }
}
