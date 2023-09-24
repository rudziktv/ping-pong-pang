using Assets.Packages.Engine.Game;
using System.Collections;
using UnityEngine;

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

        int current;

        


        private void Start()
        {
            audioSource.volume = GameSettings.MusicVolume * 0.5f;
            GameSettings.MusicVolumeChanged += MusicVolumeChanged;

            PlayQueue();
        }

        private void PlayQueue()
        {
            if (audioSource == null)
                return;
            audioSource.clip = playerQueue[current];
            audioSource.Play();
            StartCoroutine(nameof(WaitUntilEndOfSong));
        }

        IEnumerator WaitUntilEndOfSong()
        {
            current++;
            if (current >= playerQueue.Length)
                current = 0;
            yield return new WaitUntil(() => {
                if (audioSource != null)
                    return !audioSource.isPlaying;
                return true;
            });
            PlayQueue();
        }

        private void MusicVolumeChanged(float sens)
        {
            audioSource.volume = sens * 0.5f;
        }

        private void OnDestroy()
        {
            GameSettings.MusicVolumeChanged -= MusicVolumeChanged;
        }
    }
}
