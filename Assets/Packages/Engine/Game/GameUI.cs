using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }


    [SerializeField]
    UIDocument document;


    Label player1Score;
    Label player2Score;


    private void Awake()
    {
        Instance = this;

        player1Score = document.rootVisualElement.Q<Label>("player1");
        player2Score = document.rootVisualElement.Q<Label>("player2");
    }


    public void PlayerScore(OutSide side, int value)
    {
        if (side == OutSide.left)
            player1Score.text = value.ToString();
        else
            player2Score.text = value.ToString();
    }
}
