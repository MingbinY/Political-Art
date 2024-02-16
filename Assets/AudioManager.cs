using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    AudioSource audioSource;
    public AudioClip fixBenchClip;
    public AudioClip loseClip;
    public AudioClip winClip;
    public AudioClip copSeenClip;

    private void Awake()
    {
        Instance = this;
        
        audioSource = GetComponent<AudioSource>();
        if (!audioSource) gameObject.AddComponent<AudioSource>();
    }

    public void LoseClip()
    {
        audioSource.PlayOneShot(loseClip);
    }

    public void WinClip()
    {
        audioSource.PlayOneShot(winClip);
    }

    public void FixBenchClip()
    {
        audioSource.PlayOneShot(fixBenchClip);
    }

    public void CopSeenClip()
    {
        audioSource.PlayOneShot(copSeenClip);
    }
}
