using Godot;
using System;
using PingPongPang.Scripts.Networking;
using PingPongPang.Scripts.Player;

namespace PingPongPang.Scripts.Entities;

public partial class OfflinePlayer : RigidBody2D {
	
	private const float SENSITIVITY_MULTIPLIER = 800f;
	public const float BOUNDS = 150f;

	public static PlayerControlType ControlType {
		get => _controlType;
		set {
			_controlType = value;
			ControlTypeChanged?.Invoke(value);
		} 
	}

	public static float MouseSensitivity { get; set; }
	public static float KeyboardSensitivity { get; set; } = 0.42f;
	
	private static PlayerControlType _controlType;
	private static event Action<PlayerControlType> ControlTypeChanged;
	
	public Color PlayerColor { get; set; } =
		new(NetDefaultParams.DEFAULT_CLIENT_COLOR);
	
	private Sprite2D _sprite;
	private readonly Vector2[] _polygon = [
		new(-7.5f, -40f),
		new(7.5f, -40f),
		new(7.5f, 40f),
		new(-7.5f, 40f),
	];
	
	private event Action<double> ProcessFrame;
	private event Action<InputEventMouseMotion> MouseMoved;

	private void OnControlTypeChanged(PlayerControlType controlType) {
		ProcessFrame -= KeyboardMove;
		MouseMoved -= DirectMouseMove;
		MouseMoved -= RelativeMouseMove;
		
		switch (controlType) {
			case PlayerControlType.DirectMouse:
				MouseMoved += DirectMouseMove;
				break;
			case PlayerControlType.RelativeMouse:
				MouseMoved += RelativeMouseMove;
				break;
			default:
				ProcessFrame += KeyboardMove;
				break;
		}
	}
	
	public override void _Ready() {
		base._Ready();
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_sprite.Draw += DrawIt;
		ControlTypeChanged += OnControlTypeChanged;
		OnControlTypeChanged(ControlType);
	}

	public override void _Input(InputEvent @event) {
		base._Input(@event);
		if (@event is not InputEventMouseMotion mouseMotion)
			return;
		MouseMoved?.Invoke(mouseMotion);
	}

	public override void _Process(double delta) {
		base._Process(delta);
		ProcessFrame?.Invoke(delta);
	}

	private void DirectMouseMove(InputEventMouseMotion mouseMotion) {
		var vector = Position;
		vector.Y = (mouseMotion.Position.Y - GetViewportRect().Size.Y / 2f) / CameraController.CurrentZoom;
		vector.Y = Mathf.Clamp(vector.Y, -BOUNDS,  BOUNDS);
		Position = vector;
	}

	private void RelativeMouseMove(InputEventMouseMotion mouseMotion) {
		var vector = Position;
		vector.Y += mouseMotion.Relative.Y * MouseSensitivity;
		vector.Y = Mathf.Clamp(vector.Y, -BOUNDS,  BOUNDS);
		Position = vector;
	}

	private void KeyboardMove(double delta) {
		var position = Position;
		if (Input.IsActionPressed("MoveUp")) {
			position += Vector2.Up * KeyboardSensitivity * (float)delta * SENSITIVITY_MULTIPLIER;
		} else if (Input.IsActionPressed("MoveDown")) {
			position += Vector2.Down * KeyboardSensitivity * (float)delta * SENSITIVITY_MULTIPLIER;
		}
		position.Y = Mathf.Clamp(position.Y, -BOUNDS, BOUNDS);
		Position = position;
	}
	
	
	private void DrawIt() {
		_sprite.DrawColoredPolygon(_polygon, PlayerColor);
	}
}