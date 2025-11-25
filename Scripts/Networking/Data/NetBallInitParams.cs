using Godot;

namespace PingPongPang.Scripts.Networking.Data;

public partial class NetBallInitParams : GodotObject {
	public Vector2 BallDirection { get; set; }
	public double Velocity { get; set; }
	public double AccelerationRate { get; set; }
	
	// Vector2 ballDirection, float velocity, float accelerationRate
}