using Assets.Packages.Engine.Game;
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


    private void Start()
    {
        audioSource.volume = GameSettings.SFXVolume;
        GameSettings.SFXVolumeChanged += SFXVolumeChanged;
    }

    private void SFXVolumeChanged(float sens)
    {
        audioSource.volume = sens;
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
