using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Packages.Engine.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioUI : MonoBehaviour
    {
        public AudioSource AudioSource { get; private set; }

        public static AudioUI Instance { get; private set; }

        [SerializeField]
        AudioClip buttonClick;


        private void Awake()
        {
            Instance = this;
            AudioSource = GetComponent<AudioSource>();
        }

        public void PlayButtonClick()
        {
            AudioSource.PlayOneShot(buttonClick);
        }
    }
}
