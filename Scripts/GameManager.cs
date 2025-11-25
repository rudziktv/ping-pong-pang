using System;
using System.Collections.Generic;
using Godot;
using PingPongPang.Scripts.Entities;
using PingPongPang.Scripts.Networking;
using PingPongPang.Scripts.Networking.Spawn;
using PingPongPang.Scripts.Player;
using PingPongPang.Scripts.Utilities;

namespace PingPongPang.Scripts;

public partial class GameManager : Node {
	private NetLobby Lobby => NetLobby.Instance;
	private NetSpawnerPrimitive SpawnerPrimitive => NetSpawnerPrimitive.Instance;
	private NetSpawner Spawner => NetSpawner.Instance;
	public static GameManager Instance { get; set; }
	public static NetPlayer PlayerProfile { get; private set; } = new();

	public GameSession Session { get; private set; }
	
	public delegate void OnNicknameChangedArgs(string newText);
	public event OnNicknameChangedArgs OnNicknameChanged;

	public delegate void ScoreChangedArgs(NetScore score);
	public event ScoreChangedArgs ScoreChanged;
		
	public Ball Ball { get; set; }
	public OnlinePlayer HostPlayer { get; set; }
	public OnlinePlayer GuestPlayer { get; set; }
	
	private NetScore Score { get; set; }
	
	public bool HostServes => Score.HostServes;
	private int _interval;

	private AudioStreamPlayer _mainAudioPlayer;

	public AudioStream[] ImpactSounds = [];
	public AudioStream RandomImpactSound
		=> ImpactSounds[new Random().Next(0, ImpactSounds.Length)];
	
	public override void _EnterTree() {
		base._EnterTree();
		if (Instance != null) {
			QueueFree();
			return;
		}
		Instance = this;
	}

	public override void _Ready() {
		base._Ready();
		// DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen);
		_mainAudioPlayer = GetNode<AudioStreamPlayer>("/root/GameScene/AudioStreamPlayer");
		ImpactSounds = new AudioStream[5];

		for (int i = 0; i < 5; i++) {
			ImpactSounds[i] = GD.Load<AudioStream>($"res://Assets/Sounds/ImpactSounds/impactSoft_medium_00{i}.ogg");
		}
		
		// server
		Lobby.ServerCreated += OnServerCreated;
		Lobby.ServerDestroyed += OnServerDestroyed;
		
		// client
		Lobby.ConnectedToServer += OnConnectedToServer;
		Lobby.ServerDisconnected += OnServerDisconnected;
		Lobby.ServerConnectionFailed += OnServerConnectionFailed;
		
		// both
		// NetLobby.Instance.Match
		Lobby.GameMatchStarted += OnGameMatchStarted;
	}

	private void OnGameMatchStarted(NetLobbySettings lobbySettings,
		Dictionary<long, NetPlayer> players, NetMatchInitParams initParams) {
		ResetMatch();
		// HostServes = initParams.InitBallDirectionX > 0;
		Score.HostServes = MatchUtilities.IsHostServing(initParams);
		if (!Multiplayer.IsServer()) return;
		
		GD.Print($"AddPoint: {Score.HostRoundsWon} : {Score.GuestRoundsWon}. int: {_interval}, possession: {(HostServes ? "left" : "right")}");
		
		SpawnerPrimitive.SpawnAllEntities(lobbySettings, players, initParams);
		// Spawner.SpawnEntities(lobbySettings, players, initParams);
	}

	private void ResetMatch() {
		Ball?.QueueFree();
		Ball = null;
		
		HostPlayer?.QueueFree();
		HostPlayer = null;
		
		GuestPlayer?.QueueFree();
		GuestPlayer = null;
		
		Score = new NetScore();
	}

	public void SetNewNickname(string newText) {
		PlayerPrefs.SetPref("nickname", newText);
		OnNicknameChanged?.Invoke(newText);
		PlayerProfile.Nickname = newText;
	}

	public void AddPoint(PlayerSide outZoneSide) {
		_interval++;
		if (HostPlayer.Side != outZoneSide) {
			Score.HostRoundsWon++;
			if (Score.HostRoundsWon >= Lobby.LobbySettings.Turns &&
				Score.HostRoundsWon > Score.GuestRoundsWon + 1) {
				Score.HostSetsWon++;
				ResetRounds();
			}
		} else {
			Score.GuestRoundsWon++;
			if (Score.GuestRoundsWon >= Lobby.LobbySettings.Turns &&
				Score.HostRoundsWon + 1 < Score.GuestRoundsWon) {
				Score.GuestSetsWon++;
				ResetRounds();
			}
		}
		
		if (_interval >= 2) {
			_interval = 0;
			Score.HostServes = !Score.HostServes;
		}
		GD.Print($"AddPoint: {Score.HostRoundsWon} : {Score.GuestRoundsWon}. int: {_interval}, possession: {(HostServes ? "host" : "guest")}");
		NotifyClientsScoreChanged();
		ScoreChanged?.Invoke(Score);
	}

	private void ResetRounds() {
		Score.HostRoundsWon = 0;
		Score.GuestRoundsWon = 0;
	}

	private void NotifyClientsScoreChanged() {
		_mainAudioPlayer.Play();
		Rpc(MethodName.ReceiveNewScore, Score.ToJson());
	}
	
	[Rpc(TransferChannel = 8, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void ReceiveNewScore(string scoreJson) {
		Score = NetScore.FromJson(scoreJson);
		_mainAudioPlayer.Play();
		ScoreChanged?.Invoke(Score);
	}

	private void OnServerCreated(int port) {
		
	}
	
	private void OnServerDestroyed() {
		ResetMatch();
	}
	
	private void OnConnectedToServer() {
		
	}

	private void OnServerDisconnected() {
		ResetMatch();
	}
	
	private void OnServerConnectionFailed() {
		ResetMatch();
	}

	private void CleanUp() {
		ResetMatch();
	}
}