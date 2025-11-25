using System;
using Godot;

namespace PingPongPang.Scripts.Utilities;

public static class BallUtilities {

	public static Vector2 RandomizeBallDirection()
		=> RandomizeBallForward(new Random().Next() % 2 == 0);
	
	public static Vector2 RandomizeBallForward(bool facingRight) {
		var y = Mathf.Clamp(new Random().NextSingle() * 2f - 1f, -Mathf.Sqrt2 / 2f, Mathf.Sqrt2 / 2f);
		var x = Mathf.Sqrt(1f - Mathf.Pow(y, 2)) * (facingRight ? 1 : -1);
		return new Vector2(x, y);
	}
}