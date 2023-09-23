using Assets.Packages.Engine.Game;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Packages.Engine.UI
{
    public class SettingsModel
    {
        TemplateContainer tc;

        Slider p1Sensitivity;
        Slider p2Sensitivity;


        public SettingsModel(TemplateContainer tc)
        {
            this.tc = tc;

            p1Sensitivity = tc.Q<Slider>("player1sens");
            p2Sensitivity = tc.Q<Slider>("player2sens");

            p1Sensitivity.value = PlayerPrefs.GetFloat(SettingsKeys.PLAYER1_SENSITIVITY, 0.05f);
            p2Sensitivity.value = PlayerPrefs.GetFloat(SettingsKeys.PLAYER2_SENSITIVITY, 0.05f);


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


            tc.Q<Button>("reset").clicked += ResetPrefs;
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
