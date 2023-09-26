using Assets.Packages.Engine.Game;
using Assets.Packages.Engine.Game.Defaults;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Packages.Engine.UI
{
    public class GameSubpageModel
    {
        TemplateContainer tc;


        IntegerField winScore;
        EnumField initialServe;
        Slider initialVelocity;
        Toggle acceleration;
        Slider accelerationRate;


        public GameSubpageModel(TemplateContainer tc)
        {
            this.tc = tc;
            InitializeUI();
            LoadPreferences();
            RegisterCallbacks();
        }

        private void LoadPreferences()
        {
            winScore.value = PlayerPrefs.GetInt(SettingsKeys.WIN_SCORE,
                            DefaultRules.WIN_SCORE);
            initialVelocity.value = PlayerPrefs.GetFloat(SettingsKeys.INIT_VELOCITY,
                DefaultRules.INIT_VELOCITY);
            acceleration.value = PlayerPrefs.GetInt(SettingsKeys.ACCELERATION_ENABLED,
                DefaultRules.ACCELERATION_ENABLED ? 1 : 0) == 1;
            accelerationRate.value = PlayerPrefs.GetFloat(SettingsKeys.ACCELERATION_RATE,
                DefaultRules.ACCELERATION_RATE);
        }

        private void RegisterCallbacks()
        {
            winScore.RegisterValueChangedCallback((args) =>
            {
                winScore.value = args.newValue < 0 ? 0 : args.newValue;
                PlayerPrefs.SetInt(SettingsKeys.WIN_SCORE, args.newValue);
                PlayerPrefs.Save();
            });

            initialVelocity.RegisterValueChangedCallback((args) =>
            {
                PlayerPrefs.SetFloat(SettingsKeys.INIT_VELOCITY, args.newValue);
                PlayerPrefs.Save();
            });

            acceleration.RegisterValueChangedCallback((args) =>
            {
                PlayerPrefs.SetInt(SettingsKeys.ACCELERATION_ENABLED, args.newValue ? 1 : 0);
                PlayerPrefs.Save();
            });

            accelerationRate.RegisterValueChangedCallback((args) =>
            {
                PlayerPrefs.SetFloat(SettingsKeys.ACCELERATION_RATE, args.newValue);
                PlayerPrefs.Save();
            });

            SettingsModel.RestoreDefault += RestoreDefaultValues;
        }

        private void RestoreDefaultValues()
        {
            winScore.value = DefaultRules.WIN_SCORE;
            initialVelocity.value = DefaultRules.INIT_VELOCITY;
            acceleration.value = DefaultRules.ACCELERATION_ENABLED;
            accelerationRate.value = DefaultRules.ACCELERATION_RATE;
        }

        private void InitializeUI()
        {
            winScore = tc.Q<IntegerField>("winScoreSet");
            initialServe = tc.Q<EnumField>("initServe");
            initialVelocity = tc.Q<Slider>("initVelSet");
            acceleration = tc.Q<Toggle>("accelerationSet");
            accelerationRate = tc.Q<Slider>("accelerationRate");

            tc.Q<Button>("side-by-side").SetEnabled(false);

            var playBtn = tc.Q<Button>("start-game");
            playBtn.clicked += PlayButton;

            UIHelper.ButtonClickSoundSubscribe(tc);
        }

        private void PlayButton()
        {
            Setup();
            GameManager.Instance.LoadTableScene();
        }

        public void Setup()
        {
            GameRules.winScore = winScore.value;
            GameRules.velocity = initialVelocity.value;
            GameRules.startSide = (OutSide)initialServe.value;
            GameRules.accelerationOverTime = acceleration.value;
            GameRules.accelerationRate = accelerationRate.value;
        }
    }
}
