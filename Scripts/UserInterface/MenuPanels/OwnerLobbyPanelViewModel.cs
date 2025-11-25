using Godot;
using PingPongPang.Scripts.Networking;

namespace PingPongPang.Scripts.UserInterface.MenuPanels;

public class OwnerLobbyPanelViewModel : ExistingLobbyPanelViewModel {

	private CheckButton _publicSettingsToggle;

	private Button _closeLobbyButton;
	private Button _copyPortButton;
	private Button _startLobbyButton;
	
	public override void Initialize(Node root) {
		base.Initialize(root);
		_publicSettingsToggle = GetNode<CheckButton>("BoxContainer/PublicSettingsBox/PublicSettingsToggle");
		_closeLobbyButton = GetNode<Button>("BoxContainer/ActionBox/CloseLobbyButton");
		_copyPortButton = GetNode<Button>("BoxContainer/ActionBox/CopyPortButton");
		_startLobbyButton = GetNode<Button>("BoxContainer/ActionBox/StartLobbyButton");
		
		SetEnabled(true);
		
		_closeLobbyButton.Pressed += OnCloseLobbyButtonPressed;
		_copyPortButton.Pressed += OnCopyPortButtonPressed;
		_publicSettingsToggle.Toggled += OnPublicSettingsToggled;
		
		_startLobbyButton.Pressed += OnStartMatchButtonPressed;
		
		Lobby.NetPlayersUpdated += OnNetPlayersUpdated;
	}

	private void OnCopyPortButtonPressed() {
		DisplayServer.ClipboardSet(Lobby.Port.ToString());
	}

	private void OnCloseLobbyButtonPressed() {
		Lobby.DestroyServer();
	}

	public override void Destroy() {
		base.Destroy();
		_publicSettingsToggle.Toggled -= OnPublicSettingsToggled;
		_startLobbyButton.Pressed -= OnStartMatchButtonPressed;
		Lobby.NetPlayersUpdated -= OnNetPlayersUpdated;
	}

	private void OnNetPlayersUpdated(NetPlayerAction action, long id, NetPlayer player) {
		GD.Print($"PlayersCount: {Lobby.PlayersCount}, {Lobby.PlayersCount != 2}");
		_startLobbyButton.SetDisabled(Lobby.PlayersCount != 2);
	}

	private void OnStartMatchButtonPressed() {
		Lobby.StartGameMatch();
	}

	private void OnPublicSettingsToggled(bool toggledOn) {
		LobbySettings.PublicSettings = toggledOn;
		NotifyLobbySettingsChanged();
	}
	
}