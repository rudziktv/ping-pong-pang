using System;
using UnityEngine;

namespace Assets.Packages.Engine.Audio
{
    [Serializable]
    public class DynamicClip
    {
        [SerializeField]
        AudioClip clip;

        [SerializeField]
        MusicType type;

        public AudioClip Clip => clip;
        public MusicType Type => type;
    }

    public enum MusicType
    {
        calm = 0,
        normal = 1,
        dynamic = 2,
    }
}
