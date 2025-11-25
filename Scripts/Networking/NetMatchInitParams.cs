using System;
using System.Text.Json;
using Godot;

namespace PingPongPang.Scripts.Networking;

[Serializable]
public class NetMatchInitParams {

	public bool InitHostLeftSide { get; set; }
	public float InitBallDirectionX { get; set; }
	public float InitBallDirectionY { get; set; }
	
	public static NetMatchInitParams FromJson(string json)
		=> JsonSerializer.Deserialize<NetMatchInitParams>(json);
}