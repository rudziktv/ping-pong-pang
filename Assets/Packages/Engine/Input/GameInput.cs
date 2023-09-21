using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public StandardInputScheme InputScheme { get; private set; }

    public float InputAD => InputScheme.Player.TypeAD.ReadValue<float>();
    public float InputWS => InputScheme.Player.TypeWS.ReadValue<float>();
    public float InputLeftRight => InputScheme.Player.TypeLeftRight.ReadValue<float>();
    public float InputUpDown => InputScheme.Player.TypeUpDown.ReadValue<float>();



    private void Awake()
    {
        Instance = this;
        InputScheme = new();
    }

    private void OnEnable()
    {
        InputScheme.Enable();
    }

    private void OnDisable()
    {
        InputScheme.Disable();
    }
}
