using Godot;

namespace PingPongPang.Scripts.UserInterface.ViewModel.Base;

public class BaseViewModel {
	public bool Focused { get; private set; }

	protected GameViewModel GameViewModel
		=> GameViewModel.Instance;
	
	protected Node Root;

	public virtual void Initialize(Node root) {
		Root = root;
	}

	public virtual void Focus() {
		Focused = true;
	}

	public virtual void Blur() {
		Focused = false;
	}

	public virtual void Destroy() {
		Blur();
	}

	protected T GetNode<T>(NodePath path) where T : Node
		=> Root.GetNode<T>(path);
}