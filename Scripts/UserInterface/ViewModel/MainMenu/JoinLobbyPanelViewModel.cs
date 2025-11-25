using Godot;
using PingPongPang.Scripts.Networking;
using PingPongPang.Scripts.UserInterface.Model;
using PingPongPang.Scripts.UserInterface.ViewModel.Base;
using PingPongPang.Scripts.UserInterface.ViewModel.Pages;
using PingPongPang.Scripts.Utilities;

namespace PingPongPang.Scripts.UserInterface.ViewModel.MainMenu;

public class JoinLobbyPanelViewModel : TabViewModel {

	private LineEdit _nicknameEdit;
	private LineEdit _addressEdit;
	private LineEdit _portEdit;
	private Button _joinButton;

	private int _port = 9999;
	
	public override void Initialize(Node root) {
		base.Initialize(root);
		_nicknameEdit = Root.GetNode<LineEdit>("BoxContainer/NicknameBox/NicknameEdit");
		_addressEdit = Root.GetNode<LineEdit>("BoxContainer/AddressBox/AddressEdit");
		_portEdit = Root.GetNode<LineEdit>("BoxContainer/PortBox/PortEdit");
		_joinButton = Root.GetNode<Button>("BoxContainer/ActionsBox/JoinLobbyButton");

		_nicknameEdit.Text = PlayerPrefs.GetPref("nickname", "Player");
			
		_joinButton.Pressed += OnJoinButtonPressed;
		_portEdit.TextChanged += OnPortEditChanged;
	}

	private void OnPortEditChanged(string newText) {
		InputValidation.ValidatePort(_portEdit, newText, ref _port);
	}

	private void OnJoinButtonPressed() {
		NetLobby.Instance.ConnectedToServer += OnConnectedToServer;
		var res = NetLobby.Instance.JoinServer(_addressEdit.Text, _port);
		if (res == Error.Ok) return;
		NetLobby.Instance.ConnectedToServer -= OnConnectedToServer;
		GD.PrintErr(res.ToString());
	}

	private void OnConnectedToServer() {
		NetLobby.Instance.ConnectedToServer -= OnConnectedToServer;
		GameViewModel.PushTo(new GuestLobbyPageViewModel(),
			GD.Load<PackedScene>(UserInterfaceRes.GUEST_LOBBY_PAGE));
	}

	public override void Focus() {
		base.Focus();
		_nicknameEdit.Text = GameManager.PlayerProfile.Nickname;
		_nicknameEdit.TextChanged += OnNicknameEditChanged;
	}

	public override void Blur() {
		base.Blur();
		_nicknameEdit.TextChanged -= OnNicknameEditChanged;
	}

	private void OnNicknameEditChanged(string newText)
		=> GameManager.Instance.SetNewNickname(newText);
}