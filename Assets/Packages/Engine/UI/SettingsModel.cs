using Assets.Packages.Engine.Game;
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


        public SettingsModel(VisualElement tc)
        {
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
        }

        private void InitializeUI()
        {
            p1Sensitivity = tc.Q<Slider>("player1sens");
            p2Sensitivity = tc.Q<Slider>("player2sens");

            masterVolume = tc.Q<Slider>("sound-master");
            sfxVolume = tc.Q<Slider>("sound-sfx");
            musicVolume = tc.Q<Slider>("sound-music");
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
