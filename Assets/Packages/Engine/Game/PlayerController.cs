using Assets.Packages.Engine.Game;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    OutSide side;

    [SerializeField]
    float sensitivity;

    [SerializeField]
    Transform center;

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
        switch (side)
        {
            case OutSide.Left:
                input = GameInput.Instance.InputWS;
                sensitivity = GameSettings.P1Sensitivity;
                break;
            case OutSide.Right:
                input = GameInput.Instance.InputUpDown;
                sensitivity = GameSettings.P2Sensitivity;
                break;
        }
    }


    void HandleMovement()
    {
        pos = transform.position;

        pos += input * sensitivity * Vector3.up;

        pos.y = Mathf.Clamp(pos.y, -boundaries, boundaries);

        transform.position = pos;
    }


    public void CenterPlayer()
    {
        transform.position = center.position;
    }
}