using System.Text.Json;
using Godot;
using PingPongPang.Scripts.Player;

namespace PingPongPang.Scripts.Networking;

public partial class SessionSpawner : MultiplayerSpawner {
	
	private PackedScene _playerPrefab;
	
	public override void _Ready() {
		base._Ready();
		_playerPrefab = GD.Load<PackedScene>("res://Assets/Prefabs/player.tscn");
		// Multiplayer.PeerConnected += (id) => PeerConnected((int)id);
	}

	private void PeerConnected(int id) {
		if (!Multiplayer.IsServer()) return;
		SpawnPlayer(id);
	}

	private void SpawnPlayer(int id) {
		var initParams = new PlayerInitParams
		{
			AuthorityID = id,
			StartingSide = PlayerSide.Left
		};
		var json = JsonSerializer.Serialize(initParams);
		Rpc(MethodName.SpawnPlayerPerClient, json);
	}

	[Rpc(CallLocal = true)]
	private void SpawnPlayerPerClient(string json) {
		var initParams = JsonSerializer.Deserialize<PlayerInitParams>(json);
		var player = _playerPrefab.Instantiate<PlayerController>();
		player.SetClientID(initParams.AuthorityID);
		player.Name = $"Player {initParams.AuthorityID}";
		player.SetPosition(new Vector2(initParams.StartingSide == PlayerSide.Right ? 380f : -380f, 0f));
		GetNode(SpawnPath).CallDeferred("add_child", player);
	}
}