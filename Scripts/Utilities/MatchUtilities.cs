using PingPongPang.Scripts.Networking;
using PingPongPang.Scripts.Player;

namespace PingPongPang.Scripts.Utilities;

public static class MatchUtilities {

	private static GameManager Manager => GameManager.Instance;

	public static bool IsHostServing(NetMatchInitParams initParams)
		=> (initParams.InitBallDirectionX > 0 && initParams.InitHostLeftSide) ||
		   (initParams.InitBallDirectionX < 0 && !initParams.InitHostLeftSide);
	
	public static bool IsBallServeTowardsRight()
		=> (Manager.HostPlayer.Side == PlayerSide.Left && Manager.HostServes) ||
		   (Manager.GuestPlayer.Side == PlayerSide.Left && !Manager.HostServes);
}