using System;
using Godot;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text.Json;
using System.Threading.Tasks;
using PingPongPang.Scripts.Player;
using PingPongPang.Scripts.Utilities;

namespace PingPongPang.Scripts.Networking;

public partial class NetLobby : Node {
	private NetworkHandler NetworkHandler => NetworkHandler.Instance;
	public static NetLobby Instance { get; private set; }

	public int Port { get; private set; } = 9999;

	public int PlayersCount => Players.Count;
	public ImmutableDictionary<long, NetPlayer> CopyPlayers
		=> Players.ToImmutableDictionary();
	private Dictionary<long, NetPlayer> Players { get; } = [];
	public NetLobbySettings LobbySettings { get; private set; } = new();

	public delegate void ServerCreatedArgs(int port);
	public delegate void ServerDestroyedArgs();
	public delegate void ConnectedToServerArgs();
	public delegate void ServerDisconnectedArgs();
	public delegate void ServerConnectionFailedArgs();

	public delegate void GameMatchStartedArgs(NetLobbySettings lobbySettings,
		Dictionary<long, NetPlayer> players, NetMatchInitParams initParams);
	public delegate void LobbySettingsChangedArgs();
	public delegate void NetPlayersChangedArgs(NetPlayerAction action, long id, NetPlayer player);
	
	public event LobbySettingsChangedArgs LobbySettingsChanged;
	public event NetPlayersChangedArgs NetPlayersUpdated;
	public event GameMatchStartedArgs GameMatchStarted;
		
	public event ServerCreatedArgs ServerCreated;
	public event ServerDestroyedArgs ServerDestroyed;
	
	public event ConnectedToServerArgs ConnectedToServer;
	public event ServerDisconnectedArgs ServerDisconnected;
	public event ServerConnectionFailedArgs ServerConnectionFailed;

	public override void _EnterTree() {
		base._EnterTree();
		Instance = this;
	}

	public override void _Ready() {
		base._Ready();
		
		// mix side
		Multiplayer.PeerConnected += OnPlayerConnected;
		Multiplayer.PeerDisconnected += OnPlayerDisconnected;
		
		// client side
		Multiplayer.ConnectedToServer += OnConnectedToServer;
		Multiplayer.ServerDisconnected += OnServerDisconnected;
		Multiplayer.ConnectionFailed += OnServerConnectionFailed;
	}

	#region ServerSide

	public Error CreateServer(int port = NetworkHandler.PORT) {
		Port = port;
		var err = NetworkHandler.Instance.StartServer(port);
		if (err != Error.Ok)
			return err;
		GameManager.PlayerProfile.ID = Multiplayer.GetUniqueId();
		Players.Add(Multiplayer.GetUniqueId(), GameManager.PlayerProfile);
		ServerCreated?.Invoke(port);
		return Error.Ok;
	}

	public void DestroyServer() {
		if (!Multiplayer.IsServer()) return;
		Multiplayer.MultiplayerPeer = new OfflineMultiplayerPeer();
		Players.Clear();
		ServerDestroyed?.Invoke();
	}

	public void StartGameMatch() {
		if (!Multiplayer.IsServer() || Players.Count != 2) return;

		var leftSide = LobbySettings.HostStartingSide == PlayerStartingSide.Left ||
					   (LobbySettings.HostStartingSide == PlayerStartingSide.Random && new Random().Next(2) == 0);
		var vector = BallUtilities.RandomizeBallDirection();
		
		GD.Print($"BallDirection generated: {vector}");
		var initParams = new NetMatchInitParams
		{
			InitHostLeftSide = leftSide,
			InitBallDirectionX = vector.X,
			InitBallDirectionY = vector.Y
		};
		
		Rpc(MethodName.OnGameMatchStarted, LobbySettings.ToJson(), Players.ToJson(), initParams.ToJson());
	}
	
	#endregion

	#region ClientSide

	public Error JoinServer(string address = NetworkHandler.LOCALHOST_ADDRESS, int port = NetworkHandler.PORT) {
		var err = NetworkHandler.Instance.StartClient(address, port);
		if (err != Error.Ok)
			return err;
		// GameManager.PlayerProfile.ID = Multiplayer.GetUniqueId();
		// Players.Add(Multiplayer.GetUniqueId(), GameManager.PlayerProfile);
		return Error.Ok;
	}

	public void DisconnectFromServer() {
		GD.Print("Disconnect from server.");
		NetworkHandler.Peer.DisconnectPeer(1);
	}

	private void OnConnectedToServer() {
		GameManager.PlayerProfile.ID = Multiplayer.GetUniqueId();
		Players.Add(Multiplayer.GetUniqueId(), GameManager.PlayerProfile);
		Rpc(MethodName.RequestLobbySettings);
		ConnectedToServer?.Invoke();
	}
	
	private void OnServerDisconnected() {
		GD.Print("Server disconnected.");
		Players.Clear();
		ServerDisconnected?.Invoke();
	}

	private void OnServerConnectionFailed() {
		Players.Clear();
		ServerConnectionFailed?.Invoke();
	}
		
	#endregion

	#region BothSide

	private void OnPlayerConnected(long id) {
		GD.Print($"{id} Connected! - reported by {Multiplayer.GetUniqueId()}");
		if (Players.TryAdd(id, null))
			NetPlayersUpdated?.Invoke(NetPlayerAction.Connected, id, null);
		if (Multiplayer.IsServer())
			Rpc(MethodName.RequestNetPlayer);
	}
	
	private void OnPlayerDisconnected(long id) {
		Players.Remove(id, out var player);
		NetPlayersUpdated?.Invoke(NetPlayerAction.Disconnected, id, player);
	}

	[Rpc(CallLocal = true)]
	private void RequestNetPlayer() {
		var jsonNetPlayer = JsonSerializer.Serialize(GameManager.PlayerProfile);
		Rpc(MethodName.ResponseNetPlayer, jsonNetPlayer);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void ResponseNetPlayer(string jsonNetPlayer) {
		var playerProfile = JsonSerializer.Deserialize<NetPlayer>(jsonNetPlayer);
		if (Players.ContainsKey(playerProfile.ID)) {
			Players[playerProfile.ID] = playerProfile;
			NetPlayersUpdated?.Invoke(NetPlayerAction.Changed, playerProfile.ID, playerProfile);
		}
		GD.Print(jsonNetPlayer);
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void RequestLobbySettings() {
		if (!Multiplayer.IsServer()) return;
		Rpc(MethodName.ResponseLobbySettings, LobbySettings.ToJson());
	}

	[Rpc]
	private void ResponseLobbySettings(string jsonLobbySettings) {
		if (Multiplayer.IsServer()) return;
		LobbySettings = NetLobbySettings.FromJson(jsonLobbySettings);
		if (Multiplayer.GetRemoteSenderId() != Multiplayer.GetUniqueId())
			LobbySettingsChanged?.Invoke();
	}

	public void NotifyLobbySettingsChanged() {
		Rpc(MethodName.OnLobbySettingsChanged, LobbySettings.ToJson());
	}

	public async Task NotifyNetPlayersUpdated(NetPlayer player) {

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		Rpc(MethodName.ResponseNetPlayer, player.ToJson());
	}
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
	private void OnLobbySettingsChanged(string jsonNetSettings) {
		var lobbySettings = NetLobbySettings.FromJson(jsonNetSettings);
		if ((Multiplayer.GetRemoteSenderId() != 1 && !LobbySettings.PublicSettings) ||
			Multiplayer.GetRemoteSenderId() == Multiplayer.GetUniqueId())
			return;
		LobbySettings = lobbySettings;
		LobbySettingsChanged?.Invoke();
		// GD.Print($"sender: {Multiplayer.GetRemoteSenderId()}, this: {Multiplayer.GetUniqueId()} \n" + jsonNetSettings);
	}

	[Rpc(CallLocal = true)]
	private void OnGameMatchStarted(string jsonNetSettings, string jsonNetPlayers, string jsonMatchInitParams) {
		var settings = NetLobbySettings.FromJson(jsonNetSettings);
		var players = jsonNetPlayers.FromJson<Dictionary<long, NetPlayer>>();
		var initParams = NetMatchInitParams.FromJson(jsonMatchInitParams);
		// GD.Print($"Game match started: \n{jsonNetSettings}\n{jsonNetPlayers}\n{jsonMatchInitParams}");
		
		LobbySettings = settings;
		GameMatchStarted?.Invoke(settings, players, initParams);
	}

	#endregion
	
	public NetPlayer GetPlayer(long id) {
		Players.TryGetValue(id, out var player);
		return player;
	}
}