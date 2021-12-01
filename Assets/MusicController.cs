using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [Header("Other GameObjects etc.")]
    [SerializeField] AudioSource audioSource;

    [Header("Tracks")]
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip levelMusic;
    [SerializeField] private AudioClip bossMusic;

    private int currentTrack = -1;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        SetMusicTrack(0);
    }

    public void SetMusicTrack(int track)
    {
        if(currentTrack == track)
        {
            return;
        }

        currentTrack = track;

        switch (track)
        {
            case 0:
                audioSource.clip = mainMenuMusic;
                audioSource.Play();
                break;
            case 1:
                audioSource.clip = levelMusic;
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = bossMusic;
                audioSource.Play();
                break;
        }
    }
}
