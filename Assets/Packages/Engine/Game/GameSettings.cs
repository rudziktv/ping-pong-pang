using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Packages.Engine.Game
{
    public static class GameSettings
    {
		public delegate void FloatArgs(float sens);

		public static event FloatArgs P1SensitivityChanged;
		public static event FloatArgs P2SensitivityChanged;
		public static event FloatArgs SFXVolumeChanged;
		public static event FloatArgs MusicVolumeChanged;


		private static float _p1Sensitivity =
			PlayerPrefs.GetFloat(SettingsKeys.PLAYER1_SENSITIVITY, 0.05f);

		public static float P1Sensitivity
		{
			get { return _p1Sensitivity; }
			set
			{ 
				_p1Sensitivity = value;
                P1SensitivityChanged?.Invoke(P1Sensitivity);
            }
		}


		private static float _p2Sensitivity =
            PlayerPrefs.GetFloat(SettingsKeys.PLAYER2_SENSITIVITY, 0.05f);

		public static float P2Sensitivity
		{
			get { return _p2Sensitivity; }
			set
			{ 
				_p2Sensitivity = value;
                P2SensitivityChanged?.Invoke(P2Sensitivity);
            }
        }


		private static float _sfxVolume;

		public static float SFXVolume
		{
			get { return _sfxVolume; }
			set
			{ 
				_sfxVolume = value;
				SFXVolumeChanged?.Invoke(SFXVolume);
			}
		}


		private static float _musicVolume;

		public static float MusicVolume
		{
			get { return _musicVolume; }
			set
			{ 
				_musicVolume = value;
				MusicVolumeChanged?.Invoke(MusicVolume);
			}
		}

	}
}
