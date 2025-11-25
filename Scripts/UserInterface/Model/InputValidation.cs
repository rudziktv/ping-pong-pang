using Godot;

namespace PingPongPang.Scripts.UserInterface.Model;

public class InputValidation {

	public static void ValidatePort(LineEdit edit, string newText, ref int port) {
		if (!int.TryParse(newText, out var newPort)) {
			// var caretPos = edit.GetCaretColumn();
			// var length = newText.Length - port.ToString().Length;
			// edit.Text = port.ToString();
			// edit.SetCaretColumn(Mathf.Clamp(caretPos - length, 0, port.ToString().Length));
			InvalidateEdit(edit, newText, port.ToString());
			return;
		}
		port = newPort;
	}

	public static int ValidateInteger(LineEdit edit, string newText, int oldInteger) {
		if (!int.TryParse(newText, out var newInteger)) {
			var caretPos = edit.GetCaretColumn();
			var length = newText?.Length - oldInteger.ToString().Length ?? 0;
			edit.Text = oldInteger.ToString();
			edit.SetCaretColumn(Mathf.Clamp(caretPos - length, 0, oldInteger.ToString().Length));
			return oldInteger;
		}
		return newInteger;
	}

	public static void InvalidateEdit(LineEdit edit, string newText, string oldText) {
		var caretPos = edit.GetCaretColumn();
		var length = newText.Length - oldText.Length;
		edit.Text = oldText;
		edit.SetCaretColumn(Mathf.Clamp(caretPos - length, 0, oldText.Length));
	}
}