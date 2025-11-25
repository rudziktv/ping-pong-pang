using Godot;
using PingPongPang.Scripts.Networking;
using PingPongPang.Scripts.UserInterface.Model;
using PingPongPang.Scripts.UserInterface.ViewModel.Base;
using PingPongPang.Scripts.UserInterface.ViewModel.Pages;
using PingPongPang.Scripts.Utilities;

namespace PingPongPang.Scripts.UserInterface.ViewModel.MainMenu;

public class HostLobbyPanelViewModel : TabViewModel {
	private LineEdit _nicknameEdit;
	private LineEdit _portEdit;
	private Button _hostButton;
	
	private int _port = 9999;
	
	public override void Initialize(Node root) {
		base.Initialize(root);
		_nicknameEdit = Root.GetNode<LineEdit>("BoxContainer/NicknameBox/NicknameEdit");
		_portEdit = Root.GetNode<LineEdit>("BoxContainer/PortBox/PortEdit");
		_hostButton = Root.GetNode<Button>("BoxContainer/ActionsBox/HostLobbyButton");
		
		_nicknameEdit.Text = PlayerPrefs.GetPref("nickname", "Player");
		
		_hostButton.Pressed += OnHostButtonPressed;
		_portEdit.TextChanged += OnPortEditChanged;
	}

	private void OnPortEditChanged(string newText) {
		InputValidation.ValidatePort(_portEdit, newText, ref _port);
	}

	private void OnHostButtonPressed() {
		NetLobby.Instance.ServerCreated += OnServerCreated;
		var res = NetLobby.Instance.CreateServer(_port);
		if (res == Error.Ok) return;
		GD.PrintErr(res.ToString());
		NetLobby.Instance.ServerCreated -= OnServerCreated;
	}

	private void OnServerCreated(int port) {
		NetLobby.Instance.ServerCreated -= OnServerCreated;
		GameViewModel.PushTo(new OwnerLobbyPageViewModel(),
			GD.Load<PackedScene>(UserInterfaceRes.OWNER_LOBBY_PAGE));
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