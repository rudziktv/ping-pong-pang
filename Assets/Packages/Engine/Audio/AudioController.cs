using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [SerializeField]
    AudioSource audioSource;


    [Header("Clips")]


    [SerializeField]
    AudioClip scorePointClip;

    [SerializeField]
    AudioClip winClip;


    private void Awake()
    {
        Instance = this;
    }


    public void PlayScore()
    {
        audioSource.PlayOneShot(scorePointClip);
    }


    public void PlayWin()
    {
        audioSource.PlayOneShot(winClip);
    }
}
