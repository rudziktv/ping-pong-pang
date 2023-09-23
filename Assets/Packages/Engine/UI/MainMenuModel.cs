using Assets.Packages.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuModel : MonoBehaviour
{
    [SerializeField]
    UIDocument document;

    [SerializeField]
    VisualTreeAsset gameMenuAsset;

    [SerializeField]
    VisualTreeAsset settingsAsset;


    TemplateContainer gameMenuUI;
    TemplateContainer settingsUI;

    GameSubpageModel gameSubpageModel;
    SettingsModel settingsModel;


    VisualElement page;


    private void Start()
    {
        Initialize();
    }


    void Initialize()
    {
        var root = document.rootVisualElement;

        var gameBtn = root.Q<Button>("game-menu");
        var settingsBtn = root.Q<Button>("settings-menu");
        var exitBtn = root.Q<Button>("exit-button");

        page = root.Q<VisualElement>("view-area");


        gameMenuUI = gameMenuAsset.Instantiate();
        UIHelper.InitializeRoot(gameMenuUI);

        gameSubpageModel = new(gameMenuUI);

        settingsUI = settingsAsset.Instantiate();
        UIHelper.InitializeRoot(settingsUI);

        settingsModel = new(settingsUI);


        gameBtn.clicked += OpenGameMenu;
        settingsBtn.clicked += OpenSettings;
        exitBtn.clicked += Application.Quit;
    }

    private void OpenSettings()
    {
        page.Clear();
        UIHelper.HideRoot(gameMenuUI);

        page.Add(settingsUI);
        UIHelper.ShowRoot(settingsUI);
    }

    private void OpenGameMenu()
    {
        page.Clear();
        UIHelper.HideRoot(settingsUI);

        page.Add(gameMenuUI);

        UIHelper.ShowRoot(gameMenuUI);
    }
}