using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioClip WaveMusic;
    public AudioClip BossMusic;

    public AudioSource Player { get; private set; }

    public static MusicController Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Player = GetComponent<AudioSource>();
        PlayWaveMusic();
    }

    public void PlayWaveMusic()
    {
        Player.clip = WaveMusic;
        Player.loop = true;
        Player.Play();
    }

    public void PlayBossMusic()
    {
        Player.clip = BossMusic;
        Player.loop = true;
        Player.Play();
    }
}
