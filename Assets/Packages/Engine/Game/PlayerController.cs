using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    PlayerInputType inputType;

    [SerializeField]
    float sensitivity;

    [SerializeField]
    float boundaries;

    float input;

    Vector3 pos;


    private void FixedUpdate()
    {
        GetInput();
        HandleMovement();
    }


    void GetInput()
    {
        switch (inputType)
        {
            case PlayerInputType.sideArrows:
                input = GameInput.Instance.InputLeftRight;
                break;
            case PlayerInputType.verticalArrows:
                input = GameInput.Instance.InputUpDown;
                break;
            case PlayerInputType.ad:
                input = GameInput.Instance.InputAD;
                break;
            case PlayerInputType.ws:
                input = GameInput.Instance.InputWS;
                break;
        }

        Debug.Log(input);
    }


    void HandleMovement()
    {
        pos = transform.position;

        pos += input * sensitivity * Vector3.up;

        pos.y = Mathf.Clamp(pos.y, -boundaries, boundaries);

        transform.position = pos;
    }
}


public enum PlayerInputType
{
    sideArrows = 0,
    verticalArrows = 1,
    ad = 2,
    ws = 3,
    //custom = 4,
}