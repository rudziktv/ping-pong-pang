using Godot;
using PingPongPang.Scripts.Networking.Data;

namespace PingPongPang.Scripts.Networking.Spawn;

public partial class NetBallSpawner : MultiplayerSpawner {
	private GameManager Manager => GameManager.Instance;
	
	private PackedScene _ballPrefab;
	
	public override void _Ready() {
		base._Ready();
		_ballPrefab = GD.Load<PackedScene>("res://Assets/Prefabs/ball.tscn");
		SetSpawnFunction(new Callable(this, MethodName.SpawnBall));
	}
	
	private Ball SpawnBall(Variant data) {
		var initParams = data.As<NetBallInitParams>();
		var ball = _ballPrefab.Instantiate<Ball>();
		ball.SetInitParams(initParams);
		Manager.Ball = ball;

		return ball;
		// GetNode(SpawnPath).CallDeferred("add_child", ball);
	}
}