using Godot;
using PingPongPang.Scripts.UserInterface.ViewModel;
using PingPongPang.Scripts.UserInterface.ViewModel.MainMenu;

namespace PingPongPang.Scripts.UserInterface;

public partial class UserInterfaceController : Node {
	public static UserInterfaceController Instance { get; private set; }
	public GameViewModel GameViewModel { get; } = new();
	private Node _root;
	
	public override void _Ready() {
		base._Ready();
		Instance = this;
		_root = GetNode<Node>("%RootGUI");
		GameViewModel.Initialize(_root);
		GameViewModel.Focus();
		GameViewModel.PushTo(new MainMenuViewModel(),
			GD.Load<PackedScene>("res://Assets/UserInterface/main_menu_ui.tscn"));
	}
}