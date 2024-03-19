using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int EnemyKills { get; set; }

    public static PlayerStats Instance;

    private void Awake()
    {
        Instance = this;
    }
}
