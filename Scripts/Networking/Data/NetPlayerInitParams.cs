using Godot;

namespace PingPongPang.Scripts.Networking.Data;

public partial class NetPlayerInitParams : GodotObject {
	public long AuthorityID { get; set; }
	public long StartingSide { get; set; }
	public string Color { get; set; }
}