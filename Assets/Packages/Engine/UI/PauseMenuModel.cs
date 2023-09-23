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
    public class PauseMenuModel
    {
        [SerializeField]
        UIDocument pauseUI;

        VisualElement root;

        bool enabled;

        public bool Enabled => enabled;


        public void Initialize()
        {
            root = pauseUI.rootVisualElement.Q<VisualElement>("root");

            root.Q<Button>("resume").clicked += GameUI.Instance.PauseMenuClose;
            root.Q<Button>("menu").clicked += GameManager.Instance.LoadMainMenu;

            HidePauseMenu();
        }


        public void ShowPauseMenu()
        {
            enabled = true;
            root.RemoveFromClassList("hide");
        }


        public void HidePauseMenu()
        {
            enabled = false;
            root.AddToClassList("hide");
        }
    }
}
