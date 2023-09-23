using Assets.Packages.Engine.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Packages.Engine.UI
{
    [Serializable]
    public class WinScreenModel
    {
        [SerializeField]
        UIDocument document;

        VisualElement window;

        Label scorePlayer1;
        Label scorePlayer2;

        Label statsTime;
        Label statsDistance;
        Label statsContacts;


        public WinScreenModel()
        {
            Initialize();
        }

        public void Initialize()
        {
            var root = document.rootVisualElement;
            window = root.Q<VisualElement>("window");
            scorePlayer1 = root.Q<Label>("scorePlayer1");
            scorePlayer2 = root.Q<Label>("scorePlayer2");
            statsTime = root.Q<Label>("time");
            statsDistance = root.Q<Label>("distance");
            statsContacts = root.Q<Label>("contacts");

            var back = root.Q<Button>("back-to-menu");
            var play = root.Q<Button>("play-again");

            back.clicked += GoToMainMenu;
            play.clicked += PlayAgain;
        }

        private void PlayAgain()
        {
            GameController.Instance.GameRestart();
        }

        private void GoToMainMenu()
        {
            GameManager.Instance.LoadMainMenu();
        }

        public void OpenWinScreen()
        {
            var stats = GameSession.Instance.Statistics;

            Debug.Log("XD");


            statsTime.text = BuildTimeString(stats.time);
            statsDistance.text = $"{Mathf.Round(stats.distance)} m";
            statsContacts.text = $"{stats.contacts}";

            window.RemoveFromClassList("hide-window");
        }


        string BuildTimeString(float time)
        {
            string str = "";

            if (time >= 3600f)
            {
                var h = Mathf.Floor(time / 3600f);
                str += $"{h}h ";
                time %= h;
            }

            if (time >= 60f)
            {
                var min = Mathf.Floor(time / 60f);
                str += $"{min}min ";
                time %= min;
            }

            var s = Mathf.Round(time);
            str += $"{s}s";


            return str;
        }


        public void CloseWinScreen()
        {
            window.AddToClassList("hide-window");
        }
    }
}
