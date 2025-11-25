using Godot;
using PingPongPang.Scripts.Networking.Data;
using PingPongPang.Scripts.Utilities;

namespace PingPongPang.Scripts;

public partial class Ball : RigidBody2D {
	private GameManager Manager => GameManager.Instance;
	
	private float _velocity;
	private float _accelerationRate;
	private Vector2 _initDirection;
	
	private float _initVelocity;
	private Sprite2D _sprite;
	private AudioStreamPlayer2D _audioPlayer;
	private Color _color = new("#289e1d");

	private bool _netReady;

	public override void _EnterTree() {
		base._EnterTree();
		SetMultiplayerAuthority(1);
	}

	public void SetInitParams(NetBallInitParams initParams) {
		_initDirection = initParams.BallDirection;
		_initVelocity = (float)initParams.Velocity;
		_velocity = (float)initParams.Velocity;
		_accelerationRate = (float)initParams.AccelerationRate;
	}

	public void SetInitParams(Vector2 initDirection, float initVelocity, float accelerationRate) {
		_initDirection = initDirection;
		_initVelocity = initVelocity;
		_velocity = initVelocity;
		_accelerationRate = accelerationRate;
	}

	public void Rethrow() {
		if (!Multiplayer.IsServer()) return;
		_velocity = 0f;
		SetFreezeEnabled(true);
		var facingRight = MatchUtilities.IsBallServeTowardsRight();
		Teleport(new Vector2(facingRight ? -340 : 340, 0));
		GetTree().CreateTimer(2).Timeout += () => {
			_velocity = _initVelocity;
			SetFreezeEnabled(false);
			// ApplyCentralForce(BallUtilities.RandomizeBallForward(facingRight) * _velocity * 100);
			ApplyImpulse(BallUtilities.RandomizeBallForward(facingRight) * _velocity);
		};
	}

	private void Teleport(Vector2 pos) {
		LinearVelocity = Vector2.Zero;
		AngularVelocity = 0f;
		var transform = new Transform2D(0, pos);
		GlobalTransform = transform;
		PhysicsServer2D.BodySetState(GetRid(), PhysicsServer2D.BodyState.Transform, transform);
		ResetPhysicsInterpolation();
	}
	
	public override void _Ready() {
		base._Ready();
		
		_audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_sprite.Draw += OnDraw;
		
		BodyEntered += OnBodyEntered;
		
		if (!IsMultiplayerAuthority()) {
			Freeze = true;
			SetFreezeMode(FreezeModeEnum.Kinematic);
			_netReady = true;
			SendNetReady();
			return;
		}
		
		GetTree().CreateTimer(3f).Timeout += () => {
			BodyExited += OnBodyExited;
			ApplyCentralImpulse(_initDirection * _velocity);
		};
	}

	private void OnBodyEntered(Node body) {
		_audioPlayer.SetStream(Manager.RandomImpactSound);
		_audioPlayer.Play();
	}

	private void OnDraw() {
		_sprite.DrawCircle(Vector2.Zero, 13f, _color);
	}

	private void OnBodyExited(Node body) {
		if (body is not PlayerController player) return;
		var yDiff = GlobalPosition.Y - player.GlobalPosition.Y;

		var vel = LinearVelocity;
		
		vel.Y += vel.Length() * Mathf.Clamp(yDiff / 50f, -1, 1) * 0.4f;
		LinearVelocity = vel;
		
		// GD.Print($"yDiff: {yDiff}, vel: {LinearVelocity}");
		GD.Print($"Velocity: {_velocity}");
	}

	public override void _Process(double delta) {
		base._Process(delta);
		if (!IsMultiplayerAuthority()) return;
		SyncPosition();
	}

	private void SyncPosition() {
		if (!_netReady) return;
		Rpc(MethodName.SyncPosition, Position);
	}

	[Rpc(TransferMode = MultiplayerPeer.TransferModeEnum.UnreliableOrdered)]
	private void SyncPosition(Vector2 pos) {
		if (IsMultiplayerAuthority()) return;
		SetPosition(pos);
	}

	public override void _PhysicsProcess(double delta) {
		base._PhysicsProcess(delta);
		if (!IsMultiplayerAuthority() || Freeze) return;

		KeepVelocity();
		_velocity += _accelerationRate * (float)delta;
		_velocity = Mathf.Clamp(_velocity, 0, 2000f);
	}

	private void KeepVelocity() {
		var clamp = LinearVelocity / LinearVelocity.Length() * _velocity;
		if (!float.IsNaN(clamp.Y) && !float.IsNaN(clamp.X)) {
			LinearVelocity = clamp;
			LinearVelocity = LinearVelocity.LimitLength(_velocity);
		}
	}

	private void SendNetReady() {
		Rpc(MethodName.ReceiveNetReady);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void ReceiveNetReady() {
		GD.Print($"{Name} - NetReady");
		_netReady = true;
	}
}