using System;
using UnityEngine;


[Serializable]
public class GameRules
{
    public static OutSide startSide = OutSide.Left;

    public static int winScore = 3;

    public static float velocity = 2f;

    public static bool accelerationOverTime = false;

    public static float accelerationRate = 0f;

    public static OutSide SecondSide => startSide == OutSide.Left ? OutSide.Right : OutSide.Left;
}
