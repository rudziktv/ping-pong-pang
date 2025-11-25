using System.Collections.Generic;
using Godot;
using PingPongPang.Scripts.Networking;
using PingPongPang.Scripts.UserInterface.ViewModel.Base;
using PingPongPang.Scripts.UserInterface.ViewModel.MainMenu;
using PingPongPang.Scripts.UserInterface.ViewModel.Pages;

namespace PingPongPang.Scripts.UserInterface.ViewModel;

public class GameViewModel : NavigatorViewModel {

	private NetLobby Lobby => NetLobby.Instance;
	
	public static GameViewModel Instance { get; private set; }

	public override void Initialize(Node root) {
		base.Initialize(root);
		Instance = this;
		
		Lobby.GameMatchStarted += OnGameMatchStarted;
		Lobby.ServerDestroyed += LoadToMainMenu;
		Lobby.ServerDisconnected += LoadToMainMenu;
		Lobby.ServerConnectionFailed += LoadToMainMenu;
	}

	private void LoadToMainMenu() {
		PushTo(new MainMenuViewModel(), GD.Load<PackedScene>(UserInterfaceRes.MAIN_MENU_PAGE));
	}

	private void OnGameMatchStarted(NetLobbySettings lobbySettings,
		Dictionary<long, NetPlayer> players, NetMatchInitParams initParams) {

		var matchHudViewModel = new MatchHudPageViewModel();
		PushTo(matchHudViewModel,
			GD.Load<PackedScene>(UserInterfaceRes.MATCH_HUD_PAGE));
		matchHudViewModel.UpdateInitInfo(lobbySettings, players, initParams);
	}
}