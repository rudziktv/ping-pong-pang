using Godot;
using System;
using System.Text.Json;
using System.Collections.Generic;

namespace PingPongPang.Scripts.Utilities;

public static class PlayerPrefs {

	private static Dictionary<string, object> _prefs = [];

	public static void Initialize() {
		if (!FileAccess.FileExists("user://preferences.json"))
			return;

		try {
			var file = FileAccess.Open("user://preferences.json", FileAccess.ModeFlags.Read);
			if (file == null) {
				GD.PrintErr(FileAccess.GetOpenError());
				return;
			}
			var json = file.GetAsText();
			_prefs = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
			file.Close();
		}
		catch (Exception e) {
			GD.PrintErr(e);
		}
		// _prefs = 
	}

	private static void SavePrefs() {
		try {
			var file = FileAccess.Open("user://preferences.json", FileAccess.ModeFlags.Write);
			file.Resize(0);
			var json = JsonSerializer.Serialize(_prefs);
			file.StoreString(json);
			file.Close();
		}
		catch (Exception e) {
			GD.PrintErr(e);
		}
	}
	
	public static string GetPref(string key, string defaultValue = "")
		=> _prefs.GetValueOrDefault(key, defaultValue).ToString();

	public static T GetEnum<T>(string key, T defaultValue) where T : struct, Enum {
		if (_prefs.TryGetValue(key, out var value) &&
			Enum.TryParse(value.ToString(), out T result)) {
			return result;
		}
		return defaultValue;
	}

	public static T GetPref<T>(string key, T defaultValue = default) {
		if (_prefs.TryGetValue(key, out var value)) {
			return (T)value;
		}
		return defaultValue;
	}

	public static double GetDouble(string key, double defaultValue = 0) {
		if (_prefs.TryGetValue(key, out var value) &&
			double.TryParse(value.ToString(), out double result))
			return result;
		return defaultValue;
	}

	public static void SetPref<T>(string key, T value) {
		_prefs[key] = value;
		SavePrefs();
	}

	public static void SetEnum<T>(string key, T value) where T : Enum {
		_prefs[key] = value.ToString();
		SavePrefs();
	}

	public static void SetPref(string key, string value) {
		_prefs[key] = value;
		SavePrefs();
	}
}