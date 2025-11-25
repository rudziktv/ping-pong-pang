using Godot;
using PingPongPang.Scripts.Networking;

namespace PingPongPang.Scripts.UserInterface.MenuPanels;

public class GuestLobbyPanelViewModel : ExistingLobbyPanelViewModel {

	private Label _guestLobbyLabel;
	private Button _leaveButton;
	
	public override void Initialize(Node root) {
		base.Initialize(root);

		_leaveButton = GetNode<Button>("BoxContainer/ActionsBox/LeaveLobbyButton");
		_guestLobbyLabel = GetNode<Label>("BoxContainer/GuestLobbyLabel");
		
		OnLobbySettingsChanged();
		_leaveButton.Pressed += OnLeaveButtonPressed;
		Lobby.NetPlayersUpdated += OnNetPlayersUpdated;
	}

	private void OnNetPlayersUpdated(NetPlayerAction action, long id, NetPlayer player) {
		if (id != 1 || action != NetPlayerAction.Changed) return;
		_guestLobbyLabel.Text = $"{player.Nickname}'s Lobby";
		Lobby.NetPlayersUpdated -= OnNetPlayersUpdated;
	}

	protected override void OnLobbySettingsChanged() {
		base.OnLobbySettingsChanged();
		SetEnabled(LobbySettings.PublicSettings);
	}

	public override void Destroy() {
		base.Destroy();
		_leaveButton.Pressed -= OnLeaveButtonPressed;
	}

	private void OnLeaveButtonPressed() {
		Lobby.DisconnectFromServer();
	}
}