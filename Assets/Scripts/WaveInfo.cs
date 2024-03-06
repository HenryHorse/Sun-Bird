using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveInfo : MonoBehaviour
{
    public GameObject BasicEnemy;


    public List<GameManager.SpawnGroup>[] Waves;


    void Awake()
    {
        Waves = new List<GameManager.SpawnGroup>[] {
            new(new GameManager.SpawnGroup[] {
                new(BasicEnemy, 10, 20f, 0f),
                new(BasicEnemy, 20, 20f, 20f),
            }),
        };
    }
}
