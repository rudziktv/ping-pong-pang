using Godot;

namespace PingPongPang.Scripts.UserInterface.ViewModel.Base;

public class TabViewModel : BaseViewModel {

	public TabNavigatorViewModel TabNavigator { get; private set; }

	public void Initialize(Node root, TabNavigatorViewModel tabNavigator) {
		TabNavigator = tabNavigator;
		Initialize(root);
	}
}