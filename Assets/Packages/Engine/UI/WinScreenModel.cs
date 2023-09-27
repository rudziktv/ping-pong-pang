using Assets.Packages.Engine.Game;
using System;
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
        Label setPlayer1;
        Label setPlayer2;

        Label statsTime;
        Label statsDistance;
        Label statsContacts;


        bool enabled = false;

        public bool Enabled => enabled;

        public void Initialize()
        {
            if (document == null)
                Debug.Log("DOC NULL");
            var root = document.rootVisualElement;
            window = root.Q<VisualElement>("window");
            scorePlayer1 = root.Q<Label>("scorePlayer1");
            scorePlayer2 = root.Q<Label>("scorePlayer2");
            setPlayer1 = root.Q<Label>("setPlayer1");
            setPlayer2 = root.Q<Label>("setPlayer2");
            statsTime = root.Q<Label>("time");
            statsDistance = root.Q<Label>("distance");
            statsContacts = root.Q<Label>("contacts");

            var back = root.Q<Button>("back-to-menu");
            var play = root.Q<Button>("play-again");

            back.clicked += GoToMainMenu;
            play.clicked += PlayAgain;

            UIHelper.ButtonClickSoundSubscribe(root);
        }

        private void PlayAgain()
        {
            CloseWinScreen();
            GameController.Instance.GameRestart(true);
        }

        private void GoToMainMenu()
        {
            GameManager.Instance.LoadMainMenu();
        }

        public void OpenWinScreen()
        {
            enabled = true;
            var stats = GameSession.Instance.Statistics;

            var score = GameController.Instance.GetScore;

            scorePlayer1.text = score.p1.ToString();
            scorePlayer2.text = score.p2.ToString();

            setPlayer1.text = score.sp1.ToString();
            setPlayer2.text = score.sp2.ToString();

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
            enabled = false;
        }
    }
}
