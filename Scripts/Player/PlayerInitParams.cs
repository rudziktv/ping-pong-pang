using System;
using Godot;
using Godot.Collections;

namespace PingPongPang.Scripts.Player;

[Serializable]
public class PlayerInitParams {
	public int AuthorityID { get; set; }
	public PlayerSide StartingSide { get; set; }
	public string Color { get; set; }
}