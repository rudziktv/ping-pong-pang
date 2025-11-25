using System;
using System.Text.Json;

namespace PingPongPang.Scripts.Networking;

[Serializable]
public class 
	NetPlayer {
	public long ID { get; set; }
	public string Nickname { get; set; } = "Player";
	public string Color { get; set; } = "#ffffff";
	
	public string ToJson()
		=> JsonSerializer.Serialize(this);
	public static NetPlayer FromJson(string json)
		=> JsonSerializer.Deserialize<NetPlayer>(json);
}