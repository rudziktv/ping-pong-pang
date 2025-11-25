using Godot;

namespace PingPongPang.Scripts.Networking;

public partial class NetworkHandler : Node {
	public const string LOCALHOST_ADDRESS = "localhost";
	public const int PORT = 9999;
	private const int MAX_CLIENTS = 1;
	
	public static NetworkHandler Instance { get; private set; }
	
	public ENetMultiplayerPeer Peer { get; private set; }

	public override void _Ready() {
		base._Ready();
		if (Instance != null) {
			QueueFree();
			return;
		}
		Instance = this;
	}

	public Error StartServer(int port = PORT) {
		Peer = new ENetMultiplayerPeer();
		var err = Peer.CreateServer(port, MAX_CLIENTS);

		if (err == Error.Ok) {
			Multiplayer.MultiplayerPeer = Peer;
			// GD.Print($"Server created at port {PORT}.");
		} else {
			GD.PrintErr($"Error occured while creating server. {err}");
		}
		
		return err;
	}
	
	public Error StartClient(string address = LOCALHOST_ADDRESS, int port = PORT) {
		Peer = new ENetMultiplayerPeer();
		var err = Peer.CreateClient(address, port);

		if (err == Error.Ok) {
			Multiplayer.MultiplayerPeer = Peer;
			// GD.Print($"Client created at port {PORT}.");
		} else {
			GD.PrintErr($"Error occured while creating client. {err}");
		}
		
		return err;
	}
}