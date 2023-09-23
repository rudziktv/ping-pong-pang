using Assets.Packages.Engine.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }


    [SerializeField]
    UIDocument document;

    [SerializeField]
    WinScreenModel winScreenUI;


    Label player1Score;
    Label player2Score;


    private void Awake()
    {
        Instance = this;

        winScreenUI.Initialize();

        player1Score = document.rootVisualElement.Q<Label>("player1");
        player2Score = document.rootVisualElement.Q<Label>("player2");
    }


    public void PlayerScore(OutSide side, int value)
    {
        if (side == OutSide.Left)
            player1Score.text = value.ToString();
        else
            player2Score.text = value.ToString();
    }


    public void WinScreen()
    {
        winScreenUI.OpenWinScreen();
    }


    public void WinScreenClose()
    {
        winScreenUI.CloseWinScreen();
    }
}
