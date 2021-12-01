using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSetterStartOfLevel : MonoBehaviour
{
    [SerializeField] private int track;

    private void Start()
    {
        GameObject.FindGameObjectWithTag("music").GetComponent<MusicController>().SetMusicTrack(track);
    }
}
