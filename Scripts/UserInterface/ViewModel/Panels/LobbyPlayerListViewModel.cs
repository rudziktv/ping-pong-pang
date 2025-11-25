using Godot;
using PingPongPang.Scripts.Networking;
using PingPongPang.Scripts.Player;
using PingPongPang.Scripts.UserInterface.ViewModel.Base;
using PingPongPang.Scripts.Utilities;

namespace PingPongPang.Scripts.UserInterface.ViewModel.Panels;

public class LobbyPlayerListViewModel : BaseViewModel {

	private ColorPickerButton _hostColor;
	private ColorPickerButton _guestColor;
	private Label _hostNickname;
	private Label _guestNickname;
	private HBoxContainer _guestListItem;
	private OptionButton _hostStartingSideOption;
	
	private Debouncer Debouncer { get; } = new();
	
	public override void Initialize(Node root) {
		base.Initialize(root);

		_hostColor = GetNode<ColorPickerButton>("ContentContainer/BoxContainer/PlayersList/HostListItem/ColorPicker");
		_guestColor = GetNode<ColorPickerButton>("ContentContainer/BoxContainer/PlayersList/GuestListItem/ColorPicker");
		_hostNickname = GetNode<Label>("ContentContainer/BoxContainer/PlayersList/HostListItem/NicknameLabel");
		_guestNickname = GetNode<Label>("ContentContainer/BoxContainer/PlayersList/GuestListItem/NicknameLabel");
		_guestListItem = GetNode<HBoxContainer>("ContentContainer/BoxContainer/PlayersList/GuestListItem");
		_hostStartingSideOption = GetNode<OptionButton>("ContentContainer/BoxContainer/BottomContainer/HostStartingSide");
		
		var owner = NetLobby.Instance.Multiplayer.GetUniqueId() == 1;
		// GD.Print($"InitializePlayersLobby, owner: {owner}");
		if (owner)
			InitializeForHost();
		else
			InitializeForClient();
		
		NetLobby.Instance.NetPlayersUpdated += OnNetPlayersUpdated;
	}

	private void OnLobbySettingsChanged() {
		_hostStartingSideOption.Select((int)NetLobby.Instance.LobbySettings.HostStartingSide);
	}

	public override void Destroy() {
		base.Destroy();
		NetLobby.Instance.NetPlayersUpdated -= OnNetPlayersUpdated;
		
		if (!_hostColor.Disabled)
			DestroyForHost();
		else
			DestroyForClient();
	}

	private void InitializeForHost() {
		_hostStartingSideOption.SetDisabled(false);
		GameManager.PlayerProfile.Color = NetDefaultParams.DEFAULT_HOST_COLOR;
		RefreshUserList();
		_guestColor.SetDisabled(true);
		_guestListItem.Hide();
		
		_hostColor.ColorChanged += OnUserColorChanged;
		_hostStartingSideOption.ItemSelected += OnStartingSideSelected;
	}

	private void InitializeForClient() {
		GameManager.PlayerProfile.Color = NetDefaultParams.DEFAULT_CLIENT_COLOR;
		// GD.Print("InitializeForClient");
		RefreshUserList();
		_hostColor.SetDisabled(true);
		_guestColor.ColorChanged += OnUserColorChanged;
		NetLobby.Instance.LobbySettingsChanged += OnLobbySettingsChanged;
	}

	private void OnStartingSideSelected(long index) {
		NetLobby.Instance.LobbySettings.HostStartingSide = (PlayerStartingSide)index;
		NetLobby.Instance.NotifyLobbySettingsChanged();
	}

	private void DestroyForHost() {
		_hostColor.ColorChanged -= OnUserColorChanged;
		_hostStartingSideOption.ItemSelected -= OnStartingSideSelected;
	}

	private void DestroyForClient() {
		_guestColor.ColorChanged -= OnUserColorChanged;
		NetLobby.Instance.LobbySettingsChanged -= OnLobbySettingsChanged;
	}
	
	private void OnUserColorChanged(Color color) {
		Debouncer.Debounce(() => UpdateColor(color));
	}

	private void UpdateColor(Color color) {
		GameManager.PlayerProfile.Color = color.ToHtml();
		_ = NetLobby.Instance.NotifyNetPlayersUpdated(GameManager.PlayerProfile);
	}

	private void OnNetPlayersUpdated(NetPlayerAction action, long id, NetPlayer player) {
		// GD.Print("NetPlayersUpdated");
		switch (id) {
			case 1:
				RefreshHostItem(player);
				break;
			default:
				RefreshGuestItem(action, player);
				break;
		}
	}

	private void RefreshUserList() {
		// GD.Print("RefreshUserList");
		var i = 0;
		foreach (var player in NetLobby.Instance.CopyPlayers.Values) {
			if (i > 1)
				break;
			if (player.ID == 1)
				RefreshHostItem(player);
			else
				RefreshGuestItem(NetPlayerAction.Changed, player);
			i++;
		}
	}

	private void RefreshHostItem(NetPlayer player) {
		if (player == null) return;
		_hostNickname.Text = player.Nickname + " [Host]";
		_hostColor.Color = new Color(player.Color);
	}

	private void RefreshGuestItem(NetPlayerAction action, NetPlayer player) {
		if (action == NetPlayerAction.Disconnected || player == null) {
			_guestListItem.Hide();
			_guestNickname.Text = "Unknown";
			return;
		}
		if (action != NetPlayerAction.Changed) return;
		_guestListItem.Show();
		_guestNickname.Text = player.Nickname;
		_guestColor.Color = new Color(player.Color);
	}
}