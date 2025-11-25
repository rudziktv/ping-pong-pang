using System;
using Godot;

namespace PingPongPang.Scripts;

public partial class TurbulenceZone : Area2D {
	public override void _Ready() {
		base._Ready();
		BodyEntered += OnBodyEntered;
	}

	private void OnBodyEntered(Node2D body) {
		if (body is Ball ball)
			OnBallEntered(ball);
	}

	private void OnBallEntered(Ball ball) {
		if (ChanceForTurbulence())
			Turbulence(ball);
	}

	private void Turbulence(Ball ball) {
		GD.Print("Turbulence");
	}

	private bool ChanceForTurbulence()
		=> new Random().Next(100) > 79;
}