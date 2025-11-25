using System;
using Godot;
using PingPongPang.Scripts.Player;

namespace PingPongPang.Scripts;

[Serializable]
public partial class PlayerController : RigidBody2D {
	private GameManager Manager => GameManager.Instance;
	[Export] public int ClientID { get; set; } = -1;

	public PlayerSide Side { get; set; }

	private Color _color;
	private Vector2[] _polygon = [
		new(-7.5f, -40f),
		new(7.5f, -40f),
		new(7.5f, 40f),
		new(-7.5f, 40f),
	];
	private Sprite2D _sprite;
	
	private const float SENSITIVITY_MULTIPLIER = 800f;
	public const float BOUNDS = 150f;
	// private float _sensitivity = 0.42f;

	public static float Sensitivity { get; set; } = 0.42f;

	public void SetClientID(int id) {
		if (ClientID == -1)
			ClientID = id;
	}

	public void SetColor(string color) {
		_color = new Color(color);
	}

	public override void _EnterTree() {
		base._EnterTree();
		if (ClientID == -1) {
			var splitId = Name.ToString().Trim().Split(' ');
			if (splitId.Length != 2) return;
			if (int.TryParse(splitId[1], out var id)) {
				SetClientID(id);
			}
		}
		SetMultiplayerAuthority(ClientID);
	}

	public override void _Ready() {
		base._Ready();
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_sprite.Draw += DrawIt;
	}

	public override void _Process(double delta) {
		base._Process(delta);
		if (!IsMultiplayerAuthority()) return;
		var position = Position;

		if (Input.IsActionPressed("MoveUp")) {
			position += Vector2.Up * Sensitivity * (float)delta * SENSITIVITY_MULTIPLIER;
		} else if (Input.IsActionPressed("MoveDown")) {
			position += Vector2.Down * Sensitivity * (float)delta * SENSITIVITY_MULTIPLIER;
		}

		position.Y = Mathf.Clamp(position.Y, -BOUNDS, BOUNDS);
		Position = position;

		Rpc(MethodName.UpdatePosition, Position);
	}

	
	[Rpc]
	private void UpdatePosition(Vector2 pos) {
		if (IsMultiplayerAuthority()) return;
		SetPosition(pos);
	}

	private void DrawIt() {
		_sprite.DrawColoredPolygon(_polygon, _color);
	}
}