using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Packages.Engine.Audio
{
    public class MusicContext
    {
        public static AudioClip LastClip { get; set; }
        public static float LastTimer { get; set; }
    }
}
