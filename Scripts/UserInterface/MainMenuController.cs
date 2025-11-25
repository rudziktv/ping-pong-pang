using System.Linq;
using Godot;
using PingPongPang.Scripts.Networking;
using PingPongPang.Scripts.UserInterface.MenuPanels;
using PingPongPang.Scripts.UserInterface.ViewModel.Base;
using PingPongPang.Scripts.UserInterface.ViewModel.MainMenu;

namespace PingPongPang.Scripts.UserInterface;

public partial class MainMenuController : Node {

	private Button _hostMenuButton;
	private Button _joinMenuButton;
	private Button _optionsMenuButton;
	private Button _exitMenuButton;
	private TabContainer _menuTabContainer;
	
	
	private BaseViewModel[] _panelViewModels = [
		new HostLobbyPanelViewModel(), new JoinLobbyPanelViewModel(),
		new OwnerLobbyPanelViewModel(), new GuestLobbyPanelViewModel()
	];
	
	private BaseViewModel CurrentViewModel => _panelViewModels[_menuTabContainer.GetCurrentTab()];
	private BaseViewModel[] UnfocusedViewModels => 
		_panelViewModels.Where((v, i) => i != _menuTabContainer.GetCurrentTab()).ToArray();
	
	public override void _Ready() {
		base._Ready();
		_hostMenuButton = GetNode<Button>("%HostLobbyMenuButton");
		_joinMenuButton = GetNode<Button>("%JoinLobbyMenuButton");
		_optionsMenuButton = GetNode<Button>("%OptionsMenuButton");
		_exitMenuButton = GetNode<Button>("%ExitMenuButton");
		_menuTabContainer = GetNode<TabContainer>("%MenuTabContainer");
		
		_panelViewModels[0].Initialize(_menuTabContainer.GetNode<PanelContainer>("CreateLobbyPanel"));
		_panelViewModels[1].Initialize(_menuTabContainer.GetNode<PanelContainer>("JoinLobbyPanel"));
		_panelViewModels[2].Initialize(_menuTabContainer.GetNode<PanelContainer>("OwnerLobbyPanel"));
		_panelViewModels[3].Initialize(_menuTabContainer.GetNode<PanelContainer>("GuestLobbyPanel"));
		
		_hostMenuButton.Pressed += OnHostMenuPressed;
		_joinMenuButton.Pressed += OnJoinMenuPressed;
		NetLobby.Instance.ServerCreated += OnServerCreated;
		NetLobby.Instance.ConnectedToServer += OnServerConnected;
	}
	
	private void OnServerCreated(int port) {
		ChangeMenuTab(2);
	}
	
	private void OnServerConnected() {
		ChangeMenuTab(3);
	}

	private void OnJoinMenuPressed() {
		ChangeMenuTab(1);
	}

	private void OnHostMenuPressed() {
		ChangeMenuTab(0);
	}

	private void ChangeMenuTab(int index) {
		if (_menuTabContainer.CurrentTab > 1)
			return;
		
		if (_menuTabContainer.CurrentTab >= 0 && _menuTabContainer.CurrentTab < _panelViewModels.Length)
			CurrentViewModel.Blur();
		_menuTabContainer.SetCurrentTab(index);
		CurrentViewModel.Focus();
	}
}