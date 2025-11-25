using Godot;
using PingPongPang.Scripts.Networking;

namespace PingPongPang.Scripts;

public partial class UserInterfaceTest : Node {
	
	private Button _serverButton;
	private Button _clientButton;
	private Label _statusLabel;
	private LineEdit _nicknameEdit;

	public override void _Ready() {
		base._Ready();
		
		_serverButton = GetNode<Button>("%ServerButton");
		_clientButton = GetNode<Button>("%ClientButton");
		_statusLabel = GetNode<Label>("%StatusLabel");
		_nicknameEdit = GetNode<LineEdit>("%NicknameEdit");
		
		_nicknameEdit.TextChanged += text => GameManager.PlayerProfile.Nickname = text;
		
		_serverButton.Pressed += ServerButtonPressed;
		_clientButton.Pressed += ClientButtonPressed;
	}

	private void ClientButtonPressed() {
		var res = NetLobby.Instance.JoinServer();
		SetButtonsDisabled(res == Error.Ok);
		_statusLabel.Text = res.ToString();
	}

	private void ServerButtonPressed() {
		var res = NetLobby.Instance.CreateServer();
		SetButtonsDisabled(res == Error.Ok);
		_statusLabel.Text = res.ToString();
	}

	private void SetButtonsDisabled(bool enabled) {
		_serverButton.SetDisabled(enabled);
		_clientButton.SetDisabled(enabled);
	}
}