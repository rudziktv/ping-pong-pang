using Godot;
using PingPongPang.Scripts.UserInterface.MenuPanels;
using PingPongPang.Scripts.UserInterface.ViewModel.Base;
using PingPongPang.Scripts.UserInterface.ViewModel.Panels;

namespace PingPongPang.Scripts.UserInterface.ViewModel.Pages;

public class OwnerLobbyPageViewModel : ControlledViewModel {

	private OwnerLobbyPanelViewModel OwnerLobbyPanel { get; } = new();
	private LobbyPlayerListViewModel LobbyPlayerList { get; } = new();
	
	public override void Initialize(Node root) {
		base.Initialize(root);
		OwnerLobbyPanel.Initialize(GetNode<PanelContainer>("ContentBox/LobbyPanel"));
		LobbyPlayerList.Initialize(GetNode<PanelContainer>("ContentBox/LobbyPlayers"));
	}

	public override void Destroy() {
		base.Destroy();
		OwnerLobbyPanel.Destroy();
		LobbyPlayerList.Destroy();
	}
}