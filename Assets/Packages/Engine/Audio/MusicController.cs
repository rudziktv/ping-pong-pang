using Assets.Packages.Engine.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        private void Start()
        {
            audioSource.volume = GameSettings.MusicVolume;
            GameSettings.MusicVolumeChanged += MusicVolumeChanged;
        }

        private void MusicVolumeChanged(float sens)
        {
            audioSource.volume = sens;
        }
    }
}
