using System;
using UnityEngine;


[Serializable]
public class GameRules
{
    public static OutSide startSide;

    public static int winScore = 12;

    public static float velocity;

    public static bool accelerationOverTime;

    public static float accelerationRate;

    public static OutSide SecondSide => startSide == OutSide.Left ? OutSide.Right : OutSide.Left;
}
