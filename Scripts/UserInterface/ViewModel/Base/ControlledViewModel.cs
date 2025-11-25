using Godot;

namespace PingPongPang.Scripts.UserInterface.ViewModel.Base;

public class ControlledViewModel : BaseViewModel {

	protected NavigatorViewModel Navigator;
	
	public virtual void Initialize(Node root, NavigatorViewModel navigator) {
		Navigator = navigator;
		Initialize(root);
	}
}