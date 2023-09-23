using Assets.Packages.Engine.Game;
using System;
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


        public GameSubpageModel(TemplateContainer tc)
        {
            this.tc = tc;

            winScore = tc.Q<IntegerField>("winScoreSet");
            initialServe = tc.Q<EnumField>("initServe");
            initialVelocity = tc.Q<Slider>("initVelSet");
            acceleration = tc.Q<Toggle>("accelerationSet");

            tc.Q<Button>("side-by-side").SetEnabled(false);

            var playBtn = tc.Q<Button>("start-game");
            playBtn.clicked += PlayButton;
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
        }
    }
}
