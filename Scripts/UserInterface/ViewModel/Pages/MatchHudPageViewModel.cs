using System.Collections.Generic;
using System.Linq;
using Godot;
using PingPongPang.Scripts.Networking;
using PingPongPang.Scripts.UserInterface.ViewModel.Base;
using PingPongPang.Scripts.Utilities;

namespace PingPongPang.Scripts.UserInterface.ViewModel.Pages;

public class MatchHudPageViewModel : ControlledViewModel {

	private NetLobby Lobby => NetLobby.Instance;
	
	private Label _hostNickname;
	private Panel _hostColor;
	private Label _hostSets;
	private Label _hostRounds;
	private PanelContainer _hostPossession;
	
	private Label _guestNickname;
	private Panel _guestColor;
	private Label _guestSets;
	private Label _guestRounds;
	private PanelContainer _guestPossession;

	public override void Initialize(Node root) {
		base.Initialize(root);

		_hostNickname = GetNode<Label>("Scoreboard/ScoreboardGrid/HostNickname");
		_hostColor = GetNode<Panel>("Scoreboard/ScoreboardGrid/HostColor");
		_hostSets = GetNode<Label>("Scoreboard/ScoreboardGrid/HostSetsBox/HostSets");
		_hostRounds = GetNode<Label>("Scoreboard/ScoreboardGrid/HostRounds");
		_hostPossession = GetNode<PanelContainer>("Scoreboard/ScoreboardGrid/HostPossession");
		
		_guestNickname = GetNode<Label>("Scoreboard/ScoreboardGrid/GuestNickname");
		_guestColor = GetNode<Panel>("Scoreboard/ScoreboardGrid/GuestColor");
		_guestSets = GetNode<Label>("Scoreboard/ScoreboardGrid/GuestSetsBox/GuestSets");
		_guestRounds = GetNode<Label>("Scoreboard/ScoreboardGrid/GuestRounds");
		_guestPossession = GetNode<PanelContainer>("Scoreboard/ScoreboardGrid/GuestPossession");
		
		// Lobby.LobbySettings
		GameManager.Instance.ScoreChanged += UpdateScore;
	}

	public void UpdateInitInfo(NetLobbySettings lobbySettings,
		Dictionary<long, NetPlayer> players, NetMatchInitParams initParams) {

		var host = players[1];
		var guest = players.Values.FirstOrDefault(p => p.ID != 1);

		if (guest == null || host == null) {
			GD.PrintErr("Cannot find players in list!");
			return;
		}
		
		_hostNickname.Text = host.Nickname;
		_guestNickname.Text = guest.Nickname;
		
		_hostColor.GetThemeStylebox("panel").Set("bg_color", new Color(host.Color));
		_guestColor.GetThemeStylebox("panel").Set("bg_color", new Color(guest.Color));
		
		UpdateScore(0, 0, 0, 0, MatchUtilities.IsHostServing(initParams));
	}

	private void UpdateScore(NetScore score) {
		UpdateScore(
			score.HostSetsWon,
			score.HostRoundsWon,
			score.GuestSetsWon,
			score.GuestRoundsWon,
			score.HostServes);
	}

	private void UpdateScore(int hostSets, int hostRounds, int guestSets, int guestRounds, bool hostServes) {
		_hostSets.Text = hostSets.ToString();
		_hostRounds.Text = hostRounds.ToString();
		_guestSets.Text = guestSets.ToString();
		_guestRounds.Text = guestRounds.ToString();

		if (hostServes) {
			_hostPossession.ThemeTypeVariation = "ActivePossessionPointer";
			_guestPossession.ThemeTypeVariation = "PossessionPointer";
		} else {
			_hostPossession.ThemeTypeVariation = "PossessionPointer";
			_guestPossession.ThemeTypeVariation = "ActivePossessionPointer";
		}
	}
}