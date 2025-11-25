using Godot;
using PingPongPang.Scripts.Player;

namespace PingPongPang.Scripts.Entities;

public partial class OnlinePlayer : OfflinePlayer {
	public PlayerSide Side { get; set; }
	public int ClientID {
		get => _clientID;
		set {
			if (ClientID == -1)
				_clientID = value;
		}
	}

	private int _clientID = -1;
	private bool _netReady;

	public override void _EnterTree() {
		base._EnterTree();
		SetMultiplayerAuthority(ClientID);
	}

	public override void _Ready() {
		base._Ready();
		// if (!IsMultiplayerAuthority()) {
		// 	GD.Print($"{Name} - Ready on {(Multiplayer.GetUniqueId() == 1 ? "server" : "client")}");
		// 	_netReady = true;
		// 	SendNetReady();
		// }
		
		if (!Multiplayer.IsServer()) {
			GD.Print($"{Name} - Sending NetReady {(Multiplayer.GetUniqueId() == 1 ? "server" : "client")}");
			_netReady = true;
			SendNetReady();
		}
	}

	public override void _Input(InputEvent @event) {
		if (!IsMultiplayerAuthority()) return;
		base._Input(@event);
	}

	public override void _Process(double delta) {
		if (!IsMultiplayerAuthority()) return;
		base._Process(delta);
		SyncPosition();
	}

	private void SyncPosition() {
		if (!_netReady) return;
		Rpc(MethodName.SyncPosition, Position);
	}

	[Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.UnreliableOrdered)]
	private void SyncPosition(Vector2 pos) {
		if (IsMultiplayerAuthority()) return;
		Position = pos;
	}
	
	private void SendNetReady() {
		// GD.Print($"{Name} - Sending NetReady by {Multiplayer.GetUniqueId()}");
		Rpc(MethodName.ReceiveNetReady);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void ReceiveNetReady() {
		GD.Print($"{Name} - NetReady on {Multiplayer.GetUniqueId()}");
		_netReady = true;
	}
}