using System.Text.Json;

namespace PingPongPang.Scripts.Networking;

public class NetScore {
	public int HostRoundsWon { get; set; }
	public int HostSetsWon { get; set; }
	public int GuestRoundsWon { get; set; }
	public int GuestSetsWon { get; set; }
	public bool HostServes { get; set; }
	
	public static NetScore FromJson(string json)
		=> JsonSerializer.Deserialize<NetScore>(json);
}