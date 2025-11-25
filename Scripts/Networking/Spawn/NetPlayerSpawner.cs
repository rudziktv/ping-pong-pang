using Godot;
using PingPongPang.Scripts.Entities;
using PingPongPang.Scripts.Networking.Data;
using PingPongPang.Scripts.Player;

namespace PingPongPang.Scripts.Networking.Spawn;

public partial class NetPlayerSpawner : MultiplayerSpawner {
	private GameManager Manager => GameManager.Instance;
	
	private PackedScene _playerPrefab;
	
	public override void _Ready() {
		base._Ready();
		_playerPrefab = GD.Load<PackedScene>("res://Assets/Prefabs/player.tscn");
		SetSpawnFunction(new Callable(this, MethodName.SpawnPlayer));
	}

	private OnlinePlayer SpawnPlayer(Variant data) {
		var initParams = data.As<NetPlayerInitParams>();
		var player = _playerPrefab.Instantiate<OnlinePlayer>();
		
		player.ClientID = (int)initParams.AuthorityID;
		player.PlayerColor = new Color(initParams.Color);
		player.Name = $"Player {initParams.AuthorityID}";
		player.SetPosition(new Vector2((PlayerSide)initParams.StartingSide == PlayerSide.Right ? 380f : -380f, 0f));
		
		if (initParams.AuthorityID == 1)
			Manager.HostPlayer = player;
		else
			Manager.GuestPlayer = player;

		return player;
		// GetNode(SpawnPath).CallDeferred("add_child", player);
	}
}