using Assets.Packages.Engine.Game;
using Assets.Packages.Engine.Game.Defaults;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Packages.Engine.UI
{
    public class SettingsModel
    {
        public delegate void RestoreArgs();
        public static event RestoreArgs RestoreDefault;

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
            RegisterCallbacks();
            LoadPreferences();

            AudioListener.volume = masterVolume.value;
            GameSettings.SFXVolume = sfxVolume.value;
            GameSettings.MusicVolume = musicVolume.value;

            UIHelper.ButtonClickSoundSubscribe(tc);
        }

        private void LoadPreferences()
        {
            p1Sensitivity.value = PlayerPrefs.GetFloat(SettingsKeys.PLAYER1_SENSITIVITY,
                DefaultSettings.SENSITIVITY);
            p2Sensitivity.value = PlayerPrefs.GetFloat(SettingsKeys.PLAYER2_SENSITIVITY,
                DefaultSettings.SENSITIVITY);

            masterVolume.value = PlayerPrefs.GetFloat(SettingsKeys.MASTER_VOLUME,
                DefaultSettings.MASTER_VOLUME);
            sfxVolume.value = PlayerPrefs.GetFloat(SettingsKeys.SFX_VOLUME,
                DefaultSettings.SFX_VOLUME);
            musicVolume.value = PlayerPrefs.GetFloat(SettingsKeys.MUSIC_VOLUME,
                DefaultSettings.MUSIC_VOLUME);
        }

        private void RegisterCallbacks()
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

            RestoreDefault += ResetPrefs;


            tc.Q<Button>("reset").clicked += ClearPreferences;
        }

        private void ClearPreferences()
        {
            RestoreDefault?.Invoke();
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
            RestoreDefaultValues();
        }

        private void RestoreDefaultValues()
        {
            p1Sensitivity.value = DefaultSettings.SENSITIVITY;
            p2Sensitivity.value = DefaultSettings.SENSITIVITY;

            masterVolume.value = DefaultSettings.MASTER_VOLUME;
            sfxVolume.value = DefaultSettings.SFX_VOLUME;
            musicVolume.value = DefaultSettings.MUSIC_VOLUME;
        }
    }
}
