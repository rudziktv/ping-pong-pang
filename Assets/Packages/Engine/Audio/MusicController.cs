using Assets.Packages.Engine.Game;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Assets.Packages.Engine.Audio
{
    public class MusicController : MonoBehaviour
    {
        [SerializeField]
        AudioSource audioSource;

        [SerializeField]
        AudioClip[] playerQueue;

        [SerializeField]
        bool randomize;

        int currentIndex;

        AudioClip CurrentClip => playerQueue[currentIndex];

        


        private void Start()
        {
            audioSource.volume = GameSettings.MusicVolume * 0.5f;
            GameSettings.MusicVolumeChanged += MusicVolumeChanged;

            if (randomize)
            {
                Random rng = new();
                playerQueue = playerQueue.OrderBy(a => rng.Next()).ToArray();
            }

            if (MusicContext.LastClip != null)
            {
                audioSource.clip = MusicContext.LastClip;
                audioSource.time = MusicContext.LastTimer;
                audioSource.Play();
                currentIndex = Array.IndexOf(playerQueue, audioSource.clip);
                StartCoroutine(nameof(WaitUntilEndOfSong));
            }
            else
            {
                PlayQueue();
            }
        }

        private void PlayQueue()
        {
            audioSource.Stop();
            audioSource.clip = playerQueue[currentIndex];
            audioSource.time = 0f;
            audioSource.Play();
            StopAllCoroutines();
            StartCoroutine(nameof(WaitUntilEndOfSong));
        }

        IEnumerator WaitUntilEndOfSong()
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);
            NextSong();
            PlayQueue();
        }

        private void NextSong()
        {
            currentIndex++;
            if (currentIndex >= playerQueue.Length)
                currentIndex = 0;
        }

        private void MusicVolumeChanged(float sens)
        {
            audioSource.volume = sens * 0.5f;
        }

        private void OnDisable()
        {
            MusicContext.LastTimer = audioSource.time;
        }

        private void OnDestroy()
        {
            GameSettings.MusicVolumeChanged -= MusicVolumeChanged;
            MusicContext.LastClip = CurrentClip;
        }
    }
}
