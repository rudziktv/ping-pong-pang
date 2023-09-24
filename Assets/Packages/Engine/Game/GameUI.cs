using Assets.Packages.Engine.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }


    [SerializeField]
    UIDocument document;

    [SerializeField]
    WinScreenModel winScreenUI;

    [SerializeField]
    PauseMenuModel pauseMenuUI;


    Label player1Score;
    Label player2Score;

    public bool GamePaused => pauseMenuUI.Enabled;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        winScreenUI.Initialize();
        pauseMenuUI.Initialize();

        GameInput.Instance.InputScheme.Player.PauseMenu.performed += PauseMenuButton;

        player1Score = document.rootVisualElement.Q<Label>("player1");
        player2Score = document.rootVisualElement.Q<Label>("player2");
    }

    private void PauseMenuButton(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!pauseMenuUI.Enabled && !winScreenUI.Enabled)
            PauseMenu();
        else if (pauseMenuUI.Enabled)
            PauseMenuClose();
            
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


    public void PauseMenu()
    {
        if (winScreenUI.Enabled)
            return;

        Time.timeScale = 0f;
        pauseMenuUI.ShowPauseMenu();
    }


    public void PauseMenuClose()
    {
        pauseMenuUI.HidePauseMenu();
        StartCoroutine(WaitSequenceClose());
    }


    IEnumerator WaitSequenceClose()
    {
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1f;
    }
}
