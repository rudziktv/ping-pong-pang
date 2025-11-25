using System.Text.Json;

namespace PingPongPang.Scripts.Utilities;

public static class JsonExtension {

	// public static string ToJson<TK, TV>(this Dictionary<TK, TV> obj) {
	// 	return JsonSerializer.Serialize(obj);
	// }
	
	public static string ToJson<T>(this T obj) {
		return JsonSerializer.Serialize(obj);
	}
	
	public static T FromJson<T>(this string json) {
		return JsonSerializer.Deserialize<T>(json);
	}
}