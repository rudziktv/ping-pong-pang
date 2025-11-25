using System.Collections.Generic;
using System.Linq;
using Godot;
using PingPongPang.Scripts.Networking.Data;

namespace PingPongPang.Scripts.Networking.Spawn;

public partial class NetSpawner : Node {
	public static NetSpawner Instance { get; private set; }
	
	private NetBallSpawner _ballSpawner;
	private NetPlayerSpawner _playerSpawner;

	public override void _Ready() {
		base._Ready();
		Instance = this;
		_ballSpawner = GetNode<NetBallSpawner>("NetBallSpawner");
		_playerSpawner = GetNode<NetPlayerSpawner>("NetPlayerSpawner");
	}

	public void SpawnEntities(NetLobbySettings settings,
		Dictionary<long, NetPlayer> players, NetMatchInitParams initParams) {

		var ballInitParams = new NetBallInitParams
		{
			BallDirection = new Vector2(initParams.InitBallDirectionX, initParams.InitBallDirectionY),
			AccelerationRate = settings.AccelerationRate,
			Velocity = settings.InitialVelocity
		};

		var host = players[1];
		var hostInitParams = new NetPlayerInitParams
		{
			AuthorityID = 1,
			Color = host.Color,
			StartingSide = initParams.InitHostLeftSide ? 1 : 0
		};
		
		var guest = players.Values.First(p => p.ID != 1);
		var guestInitParams = new NetPlayerInitParams
		{
			AuthorityID = guest.ID,
			Color = guest.Color,
			StartingSide = initParams.InitHostLeftSide ? 1 : 0
		};
		
		_ballSpawner.Spawn(ballInitParams);
		_playerSpawner.Spawn(hostInitParams);
		_playerSpawner.Spawn(guestInitParams);
	}
}