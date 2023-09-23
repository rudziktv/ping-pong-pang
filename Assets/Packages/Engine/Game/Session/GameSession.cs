using Assets.Packages.Engine.Game.Session;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }
    public SessionStats Statistics { get; private set; } = new();


    private void Awake()
    {
        Instance = this;
    }


    void Update()
    {
        Statistics.time += Time.deltaTime;
    }


    public void ResetSession()
    {
        Statistics.time = 0f;
        Statistics.contacts = 0;
        Statistics.distance = 0f;
    }


    public void AddDistance(float value)
    {
        Statistics.distance += value;
    }


    public void AddContact()
    {
        Statistics.contacts++;
    }
}
