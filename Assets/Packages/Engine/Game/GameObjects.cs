using System;
using UnityEngine;

namespace Assets.Packages.Engine.Game
{
    [Serializable]
    public class GameObjects
    {
        public GameObject ball;
        public Transform center;
        public Transform leftServe;
        public Transform rightServe;

        public PlayerController player1;
        public PlayerController player2;
    }
}
