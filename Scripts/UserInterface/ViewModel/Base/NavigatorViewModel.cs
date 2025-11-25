using Godot;

namespace PingPongPang.Scripts.UserInterface.ViewModel.Base;

public class NavigatorViewModel : BaseViewModel {
	
	private ControlledViewModel _currentViewModel;

	public void PushTo(ControlledViewModel viewModel, PackedScene viewPrefab) {
		var view = viewPrefab.Instantiate<Control>();
		ClearChildren(Root);
		Root.AddChild(view);
		AttachToView(viewModel, view);
	}
	
	public void AttachToView(ControlledViewModel viewModel, Control view) {
		viewModel.Initialize(view, this);
		_currentViewModel?.Destroy();
		_currentViewModel = viewModel;
		_currentViewModel.Focus();
	}

	private static void ClearChildren(Node parent) {
		var nodes = parent.GetChildren();
		foreach (var node in nodes) {
			parent.RemoveChild(node);
		}
	}
}