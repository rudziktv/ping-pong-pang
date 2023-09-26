using Assets.Packages.Engine.Game;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Packages.Engine.UI
{
    public class SettingsModel
    {
        VisualElement tc;

        Slider p1Sensitivity;
        Slider p2Sensitivity;

        Slider masterVolume;
        Slider sfxVolume;
        Slider musicVolume;

        EnumField fullscreenMode;
        DropdownField display;

        bool displaySettings;


        public SettingsModel(VisualElement tc, bool displaySettings = false)
        {
            this.displaySettings = displaySettings;
            this.tc = tc;
            InitializeUI();
            SubscirbeEvents();
            LoadPreferences();

            AudioListener.volume = masterVolume.value;
            GameSettings.SFXVolume = sfxVolume.value;
            GameSettings.MusicVolume = musicVolume.value;

            UIHelper.ButtonClickSoundSubscribe(tc);
        }

        private void LoadPreferences()
        {
            p1Sensitivity.value = PlayerPrefs.GetFloat(SettingsKeys.PLAYER1_SENSITIVITY, 0.05f);
            p2Sensitivity.value = PlayerPrefs.GetFloat(SettingsKeys.PLAYER2_SENSITIVITY, 0.05f);

            masterVolume.value = PlayerPrefs.GetFloat(SettingsKeys.MASTER_VOLUME, 0.5f);
            sfxVolume.value = PlayerPrefs.GetFloat(SettingsKeys.SFX_VOLUME, 1f);
            musicVolume.value = PlayerPrefs.GetFloat(SettingsKeys.MUSIC_VOLUME, 0.25f);

            if (displaySettings)
            {

            }
        }

        private void SubscirbeEvents()
        {
            p1Sensitivity.RegisterValueChangedCallback((args) =>
            {
                GameSettings.P1Sensitivity = args.newValue;
                PlayerPrefs.SetFloat(SettingsKeys.PLAYER1_SENSITIVITY,
                    args.newValue);

                PlayerPrefs.Save();
            });

            p2Sensitivity.RegisterValueChangedCallback((args) =>
            {
                GameSettings.P2Sensitivity = args.newValue;
                PlayerPrefs.SetFloat(SettingsKeys.PLAYER2_SENSITIVITY,
                    args.newValue);

                PlayerPrefs.Save();
            });

            masterVolume.RegisterValueChangedCallback((args) =>
            {
                AudioListener.volume = args.newValue;
                PlayerPrefs.SetFloat(SettingsKeys.MASTER_VOLUME,
                    args.newValue);

                PlayerPrefs.Save();
            });

            sfxVolume.RegisterValueChangedCallback((args) =>
            {
                GameSettings.SFXVolume = args.newValue;
                PlayerPrefs.SetFloat(SettingsKeys.SFX_VOLUME,
                    args.newValue);

                PlayerPrefs.Save();
            });

            musicVolume.RegisterValueChangedCallback((args) =>
            {
                GameSettings.MusicVolume = args.newValue;
                PlayerPrefs.SetFloat(SettingsKeys.MUSIC_VOLUME,
                    args.newValue);

                PlayerPrefs.Save();
            });


            tc.Q<Button>("reset").clicked += ResetPrefs;

            if (displaySettings)
            {
                display.RegisterValueChangedCallback((display) =>
                {
                    int selectedIndex = int.Parse(display.newValue.Split(' ')[1]) - 1;
                    Display.displays[selectedIndex].Activate();
                });
                //Display.onDisplaysUpdated += DisplaysUpdate;
                
            }
        }

        private void DisplaysUpdate()
        {
            throw new System.NotImplementedException();
        }

        private void InitializeUI()
        {
            p1Sensitivity = tc.Q<Slider>("player1sens");
            p2Sensitivity = tc.Q<Slider>("player2sens");

            masterVolume = tc.Q<Slider>("sound-master");
            sfxVolume = tc.Q<Slider>("sound-sfx");
            musicVolume = tc.Q<Slider>("sound-music");

            if (displaySettings)
            {
                display = tc.Q<DropdownField>("select-display");
                display.choices = Display.displays.Select((dsp, i) => $"Display {i+1}").ToList();
                display.index = Array.IndexOf(Display.displays, Display.main);
            }
        }

        private void ResetPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();

            p1Sensitivity.value = 0.05f;
            p2Sensitivity.value = 0.05f;
        }
    }
}
