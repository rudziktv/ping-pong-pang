using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerInputType inputType;


}


public enum PlayerInputType
{
    sideArrows = 0,
    verticalArrows = 1,
    ad = 2,
    ws = 3,
    //custom = 4,
}