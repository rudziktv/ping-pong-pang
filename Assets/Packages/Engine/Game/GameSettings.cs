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
		public delegate void SensitivityArgs(float sens);

		public static event SensitivityArgs P1SensitivityChanged;
		public static event SensitivityArgs P2SensitivityChanged;


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
	}
}
