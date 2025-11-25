using Godot;
using PingPongPang.Scripts.Networking;
using PingPongPang.Scripts.UserInterface.ViewModel.Base;

namespace PingPongPang.Scripts.UserInterface.MenuPanels;

public class ExistingLobbyPanelViewModel : ControlledViewModel {
	protected NetLobbySettings LobbySettings => NetLobby.Instance.LobbySettings;
	protected NetLobby Lobby => NetLobby.Instance;
	
	protected SpinBox SetsAmountSpin;
	protected SpinBox TurnsSpin;
	protected Slider VelocitySlider;
	protected Slider AccelerationSlider;

	public override void Initialize(Node root) {
		base.Initialize(root);
		SetsAmountSpin = GetNode<SpinBox>("BoxContainer/SharedLobbySettings/SetsAmountBox/SetsAmountSpin");
		TurnsSpin = GetNode<SpinBox>("BoxContainer/SharedLobbySettings/TurnsBox/TurnsSpin");
		VelocitySlider = GetNode<Slider>("BoxContainer/SharedLobbySettings/InitialVelocityBox/InitialVelocitySlider");
		AccelerationSlider = GetNode<Slider>("BoxContainer/SharedLobbySettings/AccelerationRateBox/AccelerationRateSlider");

		SetsAmountSpin.ValueChanged += OnSetsAmountChanged;
		TurnsSpin.ValueChanged += OnTurnsChanged;
		VelocitySlider.DragEnded += OnVelocityDragEnded;
		AccelerationSlider.DragEnded += OnAccelerationDragEnded;
		
		Lobby.LobbySettingsChanged += OnLobbySettingsChanged;
	}

	protected virtual void OnLobbySettingsChanged() {
		SetsAmountSpin.ValueChanged -= OnSetsAmountChanged;
		TurnsSpin.ValueChanged -= OnTurnsChanged;
		
		SetsAmountSpin.Value = LobbySettings.SetsAmount;
		TurnsSpin.Value = LobbySettings.Turns;
		
		SetsAmountSpin.ValueChanged += OnSetsAmountChanged;
		TurnsSpin.ValueChanged += OnTurnsChanged;
		
		VelocitySlider.Value = LobbySettings.InitialVelocity;
		AccelerationSlider.Value = LobbySettings.AccelerationRate;
	}
	
	private void OnSetsAmountChanged(double value) {
		LobbySettings.SetsAmount = (int)value;
		NotifyLobbySettingsChanged();
	}

	private void OnTurnsChanged(double value) {
		LobbySettings.Turns = (int)value;
		NotifyLobbySettingsChanged();
	}
	
	private void OnVelocityDragEnded(bool valueChanged) {
		if (!valueChanged) return;
		LobbySettings.InitialVelocity = (float)VelocitySlider.Value;
		NotifyLobbySettingsChanged();
	}
	
	private void OnAccelerationDragEnded(bool valueChanged) {
		if (!valueChanged) return;
		LobbySettings.AccelerationRate = (float)AccelerationSlider.Value;
		NotifyLobbySettingsChanged();
	}
	
	protected void SetEnabled(bool enabled) {
		SetsAmountSpin.SetEditable(enabled);
		TurnsSpin.SetEditable(enabled);
		VelocitySlider.SetEditable(enabled);
		AccelerationSlider.SetEditable(enabled);
	}

	protected void NotifyLobbySettingsChanged()
		=> Lobby.NotifyLobbySettingsChanged();
}