using Assets.Packages.Engine.UI;
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

    [SerializeField]
    VisualTreeAsset credits;


    TemplateContainer gameMenuUI;
    TemplateContainer settingsUI;
    TemplateContainer creditsUI;

    GameSubpageModel gameSubpageModel;
    SettingsModel settingsModel;


    VisualElement page;
    Label version;
    Label productName;
    Label author;


    private void Start()
    {
        Initialize();
    }


    void Initialize()
    {
        var root = document.rootVisualElement;

        var gameBtn = root.Q<Button>("game-menu");
        var settingsBtn = root.Q<Button>("settings-menu");
        root.Q<Button>("credits").clicked += CreditsShow;
        var exitBtn = root.Q<Button>("exit-button");

        page = root.Q<VisualElement>("view-area");
        version = root.Q<Label>("version");
        productName = root.Q<Label>("product-name");
        author = root.Q<Label>("author");

        version.text = $"{Application.version} - {Application.buildGUID}";
        productName.text = Application.productName;
        author.text = Application.companyName;


        gameMenuUI = gameMenuAsset.Instantiate();
        UIHelper.InitializeRoot(gameMenuUI);

        gameSubpageModel = new(gameMenuUI);

        settingsUI = settingsAsset.Instantiate();
        UIHelper.InitializeRoot(settingsUI);

        creditsUI = credits.Instantiate();
        UIHelper.InitializeRoot(creditsUI);

        settingsModel = new(settingsUI, true);


        gameBtn.clicked += OpenGameMenu;
        settingsBtn.clicked += OpenSettings;
        exitBtn.clicked += Application.Quit;

        UIHelper.ButtonClickSoundSubscribe(root);
    }

    private void CreditsShow()
    {
        page.Clear();
        UIHelper.HideRoot(gameMenuUI);
        UIHelper.HideRoot(settingsUI);

        page.Add(creditsUI);
        UIHelper.ShowRoot(creditsUI);

    }

    private void OpenSettings()
    {
        page.Clear();
        UIHelper.HideRoot(gameMenuUI);
        UIHelper.HideRoot(creditsUI);

        page.Add(settingsUI);
        UIHelper.ShowRoot(settingsUI);
    }

    private void OpenGameMenu()
    {
        page.Clear();
        UIHelper.HideRoot(settingsUI);
        UIHelper.HideRoot(creditsUI);

        page.Add(gameMenuUI);

        UIHelper.ShowRoot(gameMenuUI);
    }
}