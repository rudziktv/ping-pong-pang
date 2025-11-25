using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Godot;
using PingPongPang.Scripts.Entities;
using PingPongPang.Scripts.Player;

namespace PingPongPang.Scripts.Networking;

public partial class NetSpawnerPrimitive : MultiplayerSpawner {
	private GameManager Manager => GameManager.Instance;
	public static NetSpawnerPrimitive Instance { get; private set; }
	
	private PackedScene _ballPrefab;
	private PackedScene _playerPrefab;

	public override void _Ready() {
		base._Ready();
		Instance = this;
		_ballPrefab = GD.Load<PackedScene>("res://Assets/Prefabs/ball.tscn");
		_playerPrefab = GD.Load<PackedScene>("res://Assets/Prefabs/online_player.tscn");
	}

	public void SpawnAllEntities(NetLobbySettings settings,
		Dictionary<long, NetPlayer> players, NetMatchInitParams initParams) {
		var guest = players.Values.First(p => p.ID != 1);
		
		SpawnPlayer(1, initParams.InitHostLeftSide ? PlayerSide.Left : PlayerSide.Right, players[1].Color);
		SpawnPlayer((int)guest.ID, !initParams.InitHostLeftSide ? PlayerSide.Left : PlayerSide.Right,
			guest.Color);
		SpawnBall(new Vector2(initParams.InitBallDirectionX, initParams.InitBallDirectionY),
			settings.InitialVelocity, settings.AccelerationRate);
	}

	private void SpawnPlayer(int id, PlayerSide side, string color) {
		var initParams = new PlayerInitParams
		{
			AuthorityID = id,
			StartingSide = side,
			Color = color
		};
		var json = JsonSerializer.Serialize(initParams);
		var err = Rpc(MethodName.SpawnPlayerPerClient, json);
		
		if (err != Error.Ok)
			GD.PrintErr($"Error, SpawnPlayer: {err}");
	}

	[Rpc(CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void SpawnPlayerPerClient(string json) {
		var initParams = JsonSerializer.Deserialize<PlayerInitParams>(json);
		var player = _playerPrefab.Instantiate<OnlinePlayer>();
		
		player.ClientID = initParams.AuthorityID;
		player.PlayerColor = new Color(initParams.Color);
		player.Name = $"Player {initParams.AuthorityID}";
		player.Side = initParams.StartingSide;
		player.SetPosition(new Vector2(initParams.StartingSide == PlayerSide.Right ? 380f : -380f, 0f));
		
		if (initParams.AuthorityID == 1)
			Manager.HostPlayer = player;
		else
			Manager.GuestPlayer = player;
		
		GetNode(SpawnPath).CallDeferred("add_child", player);
	}

	private void SpawnBall(Vector2 ballDirection, float velocity, float accelerationRate) {
		Rpc(MethodName.SpawnBallPerClient, ballDirection, velocity, accelerationRate);
	}

	[Rpc(CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void SpawnBallPerClient(Vector2 ballDirection, float velocity, float accelerationRate) {
		var ball = _ballPrefab.Instantiate<Ball>();
		ball.SetInitParams(ballDirection, velocity, accelerationRate);
		ball.SetPosition(new Vector2(ballDirection.X > 0 ? -340f : 340f, 0));
		Manager.Ball = ball;
		GetNode(SpawnPath).CallDeferred("add_child", ball);
	}
}