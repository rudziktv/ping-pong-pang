using Godot;
using PingPongPang.Scripts.Controllers;
using PingPongPang.Scripts.Player;
using PingPongPang.Scripts.UserInterface.ViewModel.Base;

namespace PingPongPang.Scripts.UserInterface.ViewModel.MainMenu;

public class OptionsPanelViewModel : TabViewModel {

	private ColorPickerButton _backgroundColorPicker;
	private OptionButton _playerControlTypeOption;
	private Slider _keyboardSensitivity;
	private Slider _mouseSensitivity;
	
	public override void Initialize(Node root) {
		base.Initialize(root);

		_backgroundColorPicker = GetNode<ColorPickerButton>("BoxContainer/BackgroundColorBox/BackgroundColorPicker");
		_playerControlTypeOption = GetNode<OptionButton>("BoxContainer/ControlTypeBox/PlayerControlTypeOption");
		_keyboardSensitivity = GetNode<Slider>("BoxContainer/KeyboardSensitivityBox/KeyboardSensitivitySlider");
		_mouseSensitivity = GetNode<Slider>("BoxContainer/MouseSensitivityBox/MouseSensitivitySlider");

		_backgroundColorPicker.Color = GameOptions.BackgroundColor;
		_backgroundColorPicker.ColorChanged += OnBackgroundColorPicked;

		_playerControlTypeOption.Selected = (int)GameOptions.ControlType;
		_playerControlTypeOption.ItemSelected += OnPlayerControlTypeSelected;
		
		_keyboardSensitivity.Value = GameOptions.KeyboardSensitivity;
		_keyboardSensitivity.DragEnded += OnKeyboardSensitivityChanged;
		
		_mouseSensitivity.Value = GameOptions.MouseSensitivity;
		_mouseSensitivity.DragEnded += OnMouseSensitivityChanged;
		
		OnPlayerControlTypeSelected((long)GameOptions.ControlType);
	}

	private void SetEditableSensitivitySliders(bool keyboard, bool mouse) {
		_keyboardSensitivity.SetEditable(keyboard);
		_mouseSensitivity.SetEditable(mouse);
	}
	
	private void OnBackgroundColorPicked(Color color) {
		GameOptions.BackgroundColor = color;
	}
	
	private void OnPlayerControlTypeSelected(long index) {
		GameOptions.ControlType = (PlayerControlType)index;
		switch (GameOptions.ControlType) {
			case PlayerControlType.DirectMouse:
				SetEditableSensitivitySliders(false, false);
				break;
			case PlayerControlType.RelativeMouse:
				SetEditableSensitivitySliders(false, true);
				break;
			default:
				SetEditableSensitivitySliders(true, false);
				break;
		}
	}

	private void OnKeyboardSensitivityChanged(bool valueChanged) {
		GameOptions.KeyboardSensitivity = _keyboardSensitivity.Value;
	}
	
	private void OnMouseSensitivityChanged(bool valueChanged) {
		GameOptions.MouseSensitivity = _mouseSensitivity.Value;
	}
}