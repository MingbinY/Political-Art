using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    AudioSource audioSource;
    public AudioClip defaultClip;
    public AudioClip chasingClip;
    public AudioClip progressionClip;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = defaultClip;
        audioSource.Play();
        audioSource.loop = true;
    }

    public void Chasing()
    {
        if (audioSource.clip != chasingClip)
        {
            audioSource.clip = chasingClip;
            audioSource.Play();
        }
    }

    public void Ending()
    {
        audioSource.clip = chasingClip;
        audioSource.Play();
    }
}
