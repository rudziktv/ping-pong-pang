using Godot;

namespace PingPongPang.Scripts.Entities;

public partial class CameraController : Camera2D {
	
	private const float MINIMAL_VERTICAL_SPACE = 480;
	private const float MINIMAL_HORIZONTAL_SPACE = 960;

	public static float CurrentZoom { get; set; }
	
	public override void _Ready() {
		base._Ready();
		OnSizeChanged();
		GetViewport().SizeChanged += OnSizeChanged;
	}

	private void OnSizeChanged() {
		var windowSize = GetViewportRect().Size;
		var yZoom = Mathf.Clamp(windowSize.Y / MINIMAL_VERTICAL_SPACE, 0.2f, 32f);
		var xZoom = Mathf.Clamp(windowSize.X / MINIMAL_HORIZONTAL_SPACE, 0.2f, 32f);
		CurrentZoom = Mathf.Min(yZoom, xZoom);
		SetZoom(new Vector2(CurrentZoom, CurrentZoom));
	}
}