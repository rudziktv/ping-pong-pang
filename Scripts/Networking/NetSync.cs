using Godot;

namespace PingPongPang.Scripts.Networking;

public partial class NetSync : Node {
	private GameManager Manager => GameManager.Instance;
	
	public override void _Ready() {
		base._Ready();
	}
	
	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (Multiplayer.IsServer())
			SendTickToClients();
	}

	private void SendTickToClients() {
		Rpc(MethodName.SyncWithServer,
			Manager.Ball.Position,
			Manager.HostPlayer.Position);
	}

	private void SendToServer() {
		Rpc(MethodName.SyncWithServer,
			Manager.GuestPlayer.Position);
	}

	[Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.UnreliableOrdered)]
	private static void SyncWithServer(Vector2 ballPos, Vector2 hostPos) {
		
	}
	
	[Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.UnreliableOrdered)]
	private static void SyncWithClient(Vector2 clientPos) {
		
	}
}