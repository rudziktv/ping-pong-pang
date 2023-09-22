using System;
using UnityEngine;


[Serializable]
public class GameRules
{
    public OutSide startSide;

    public int winScore = 12;

    [Range(1f, 10f)]
    public float velocity;

    public OutSide SecondSide => startSide == OutSide.left ? OutSide.right : OutSide.left;
}
