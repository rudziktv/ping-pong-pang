using Assets.Packages.Engine.Game;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Assets.Packages.Engine.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class DynamicMusicController : MonoBehaviour
    {
        [SerializeField]
        DynamicClip[] playlist;

        [SerializeField]
        float normalRange;

        [SerializeField]
        float dynamicRange;

        [SerializeField]
        AnimationCurve fadeCurve;

        [SerializeField]
        float fadeTime;

        AudioSource audioSource;

        DynamicClip[] queue;
        int currentIndex;

        AudioClip CurrentClip => queue[currentIndex].Clip;

        MusicType currentType;



        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            GameSettings.MusicVolumeChanged += MusicVolumeChanged;
            audioSource.volume = GameSettings.MusicVolume * 0.5f;

            SetupQueueSimple();
            LoopCoroutine();
        }

        private void CheckQueueType()
        {
            if (currentType != GetCurrentType())
            {
                SetupQueue();
            }
        }

        void SetupQueue()
        {
            currentType = GetCurrentType();
            queue = GetClipsOfType(currentType);
            
            currentIndex = 0;
            if (audioSource.isPlaying)
            {
                StartCoroutine(nameof(FadeToNextSong));
            }
            else
            {
                PlaySong();
            }
        }

        void SetupQueueSimple()
        {
            currentType = GetCurrentTypeSimple();
            queue = GetClipsOfType(currentType);

            currentIndex = 0;
            if (audioSource.isPlaying)
            {
                StartCoroutine(nameof(FadeToNextSong));
            }
            else
            {
                PlaySong();
            }
        }

        IEnumerator FadeToNextSong()
        {
            float timer = 0f;

            while (timer < fadeTime)
            {
                audioSource.volume = fadeCurve
                    .Evaluate(1f - timer / fadeTime) * 0.5f * GameSettings.MusicVolume;
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            timer = 0f;
            PlaySong();
            while (timer < fadeTime)
            {
                audioSource.volume = fadeCurve
                    .Evaluate(timer / fadeTime) * 0.5f * GameSettings.MusicVolume;
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        private void PlaySong()
        {
            audioSource.Stop();
            audioSource.clip = CurrentClip;
            audioSource.Play();
        }

        MusicType GetCurrentType()
        {
            if (GameController.Instance.IntentionalVelocity > dynamicRange)
                return MusicType.dynamic;
            else if (GameController.Instance.IntentionalVelocity > normalRange)
                return MusicType.normal;
            else
                return MusicType.calm;
        }

        MusicType GetCurrentTypeSimple()
        {
            if (GameRules.velocity > dynamicRange
                    || GameRules.accelerationOverTime)
                return MusicType.dynamic;
            else if (GameRules.velocity > normalRange)
                return MusicType.normal;
            else
                return MusicType.calm;
        }

        void NextSong()
        {
            currentIndex++;
            if (currentIndex >= queue.Length)
                currentIndex = 0;
        }

        DynamicClip[] GetClipsOfType(MusicType type)
        {
            Random rng = new();
            return playlist.Where((clip) => type == clip.Type).OrderBy(a => rng.Next()).ToArray();
        }

        IEnumerator PlayerCoroutine()
        {
            yield return new WaitUntil(() =>
            {
                //CheckQueueType();
                return !audioSource.isPlaying;
            });

            NextSong();
            PlaySong();
            LoopCoroutine();
        }

        private void LoopCoroutine()
        {
            StartCoroutine(nameof(PlayerCoroutine));
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
