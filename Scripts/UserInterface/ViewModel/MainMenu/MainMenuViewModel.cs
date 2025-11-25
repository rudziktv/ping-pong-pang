using Godot;
using PingPongPang.Scripts.Entities;
using PingPongPang.Scripts.UserInterface.ViewModel.Base;

namespace PingPongPang.Scripts.UserInterface.ViewModel.MainMenu;

public class MainMenuViewModel : ControlledViewModel {

	private const int HOST_MENU_TAB = 0;
	private const int JOIN_MENU_TAB = 1;
	private const int OPTIONS_MENU_TAB = 2;
	
	private Button _hostMenuButton;
	private Button _joinMenuButton;
	private Button _optionsMenuButton;
	private Button _exitMenuButton;
	private TabContainer _menuTabContainer;
	
	private TabNavigatorViewModel MenuTabNavigator { get; } = new();
	
	private Panel _menuBackground;

	private PackedScene _offlinePlayerPrefab;
	private OfflinePlayer _offlinePlayer;

	public override void Initialize(Node root) {
		base.Initialize(root);

		_offlinePlayerPrefab = GD.Load<PackedScene>("res://Assets/Prefabs/offline_player.tscn");

		_menuBackground = GetNode<Panel>("Panel");
		_hostMenuButton = GetNode<Button>("%HostLobbyMenuButton");
		_joinMenuButton = GetNode<Button>("%JoinLobbyMenuButton");
		_optionsMenuButton = GetNode<Button>("%OptionsMenuButton");
		_exitMenuButton = GetNode<Button>("%ExitMenuButton");
		_menuTabContainer = GetNode<TabContainer>("%MenuTabContainer");
		
		MenuTabNavigator.Initialize(_menuTabContainer, [
			new HostLobbyPanelViewModel(),
			new JoinLobbyPanelViewModel(),
			new OptionsPanelViewModel()
		]);
		
		MenuTabNavigator.TabChanged += OnTabChanged;

		_hostMenuButton.Pressed += () => {
			MenuTabNavigator.SetCurrentTab(HOST_MENU_TAB);
		};

		_joinMenuButton.Pressed += () => {
			MenuTabNavigator.SetCurrentTab(JOIN_MENU_TAB);
		};
		
		_optionsMenuButton.Pressed += OpenOptionsTab;
		
		// _optionsMenuButton.SetDisabled(true);
		_exitMenuButton.Pressed += OnExitButtonPressed;
	}

	private void OnTabChanged(int index) {
		if (index != OPTIONS_MENU_TAB) {
			_menuBackground.GetThemeStylebox("panel").
				Set("bg_color", new Color("#67a0c8"));
			_offlinePlayer?.QueueFree();
			_offlinePlayer = null;
			return;
		}
		
		_menuBackground.GetThemeStylebox("panel").
			Set("bg_color", new Color("#00000000"));

		_offlinePlayer = _offlinePlayerPrefab.Instantiate<OfflinePlayer>();
		_offlinePlayer.Position = new Vector2(300, 0);
		GetNode<Node2D>("/root/GameScene/SpawnPoint").AddChild(_offlinePlayer);
	}

	private void OpenOptionsTab() {
		MenuTabNavigator.SetCurrentTab(OPTIONS_MENU_TAB);
	}

	public override void Destroy() {
		base.Destroy();
		_exitMenuButton.Pressed -= OnExitButtonPressed;
	}

	private void OnExitButtonPressed() {
		Root.GetTree().Quit();
	}
}