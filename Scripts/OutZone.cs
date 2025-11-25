using System.Threading.Tasks;
using Godot;
using PingPongPang.Scripts.Player;

namespace PingPongPang.Scripts;

public partial class OutZone : Area2D {
	private GameManager Manager => GameManager.Instance;
	[Export] public PlayerSide OutZoneSide { get; set; }
	
	public override void _Ready() {
		base._Ready();
		if (!Multiplayer.IsServer()) return;
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body) {
		if (!Multiplayer.IsServer()) return;
		
		if (body is Ball ball)
			OnBallEntered(ball);
	}

	private void OnBallEntered(Ball ball) {
		GD.Print($"{ball.Name} entered on {Name}");
		Manager.AddPoint(OutZoneSide);
		var timer = GetTree().CreateTimer(1);
		timer.Timeout += OnTimeout;

		void OnTimeout() {
			DelayedBallRethrow(ball);
			timer.Timeout -= OnTimeout;
		}
	}

	private void DelayedBallRethrow(Ball ball) {
		GD.Print($"Rethrowing {ball.Name}");
		ball.Rethrow();
	}
}