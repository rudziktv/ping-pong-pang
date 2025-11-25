using Godot;
using PingPongPang.Scripts.Entities;
using PingPongPang.Scripts.Player;
using PingPongPang.Scripts.Utilities;

namespace PingPongPang.Scripts.Controllers;

public partial class GameOptions : Node {
	private const string KEYBOARD_SENSITIVITY = "keyboard_sensitivity";
	private const string MOUSE_SENSITIVITY = "mouse_sensitivity";
	private const string CONTROL_TYPE = "control_type";
	private const string BACKGROUND_COLOR = "background_color";
	
	public static Color BackgroundColor {
		get => new(PlayerPrefs.GetPref(BACKGROUND_COLOR, "#0c0c0c"));
		set {
			PlayerPrefs.SetPref(BACKGROUND_COLOR, value.ToHtml(false));
			RenderingServer.SetDefaultClearColor(value);
		}
	}
	
	public static PlayerControlType ControlType {
		get => PlayerPrefs.GetEnum(CONTROL_TYPE, PlayerControlType.Keyboard);
		set {
			PlayerPrefs.SetEnum(CONTROL_TYPE, value);
			OfflinePlayer.ControlType = value;
		}
	}
	
	public static double KeyboardSensitivity {
		get => PlayerPrefs.GetDouble(KEYBOARD_SENSITIVITY, 0.42);
		set {
			PlayerPrefs.SetPref(KEYBOARD_SENSITIVITY, value);
			PlayerController.Sensitivity = (float)value;
			OfflinePlayer.KeyboardSensitivity = (float)value;
		}
	}
	
	public static double MouseSensitivity {
		get => (float)PlayerPrefs.GetDouble(MOUSE_SENSITIVITY, 1f);
		set {
			PlayerPrefs.SetPref(MOUSE_SENSITIVITY, value);
			OfflinePlayer.MouseSensitivity = (float)value;
		}
	}
	
	public override void _Ready() {
		base._Ready();
		Initialize();
	}

	private static void Initialize() {
		PlayerPrefs.Initialize();
		LoadPreferences();
	}

	private static void LoadPreferences() {
		RenderingServer.SetDefaultClearColor(BackgroundColor);
		PlayerController.Sensitivity = (float)KeyboardSensitivity;
		OfflinePlayer.KeyboardSensitivity = (float)KeyboardSensitivity;
		OfflinePlayer.MouseSensitivity = (float)MouseSensitivity;
		OfflinePlayer.ControlType = ControlType;
		GameManager.PlayerProfile.Nickname = PlayerPrefs.GetPref("nickname", "Player");
	}
}