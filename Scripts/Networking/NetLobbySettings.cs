using System;
using System.Text.Json;
using PingPongPang.Scripts.Player;

namespace PingPongPang.Scripts.Networking;

[Serializable]
public class NetLobbySettings {
	public bool PublicSettings { get; set; }
	public int SetsAmount { get; set; } = 3;
	public int Turns { get; set; } = 12;
	public float InitialVelocity { get; set; } = 250f;
	public float AccelerationRate { get; set; } = 2f;
	public PlayerStartingSide HostStartingSide { get; set; } = PlayerStartingSide.Random;

	public string ToJson()
		=> JsonSerializer.Serialize(this);
	public static NetLobbySettings FromJson(string json)
		=> JsonSerializer.Deserialize<NetLobbySettings>(json);
}