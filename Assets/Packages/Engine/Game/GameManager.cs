using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Packages.Engine.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }


        private void Awake()
        {
            Instance = this;
        }


        public void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }


        public void LoadTableScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}
