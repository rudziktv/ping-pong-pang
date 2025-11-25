using System.Collections.Generic;
using Godot;

namespace PingPongPang.Scripts.UserInterface.ViewModel.Base;

public class TabNavigatorViewModel : BaseViewModel {

	private TabContainer _tabContainer;
	private int CurrentIndex => _tabContainer.GetCurrentTab();
	private TabViewModel CurrentViewModel
		=> CurrentIndex < 0 || CurrentIndex >= Tabs.Count ? null : Tabs[CurrentIndex];
	public List<TabViewModel> Tabs { get; } = [];

	public event TabChangedArgs TabChanged;
	public delegate void TabChangedArgs(int index);

	public virtual void Initialize(TabContainer tabContainer, List<TabViewModel> tabs) {
		Initialize(tabContainer);
		Tabs.AddRange(tabs);
		
		var tabsView = tabContainer.GetChildren();
		for (int i = 0; i < tabs.Count; i++) {
			if (i >= tabsView.Count)
				break;
			Tabs[i].Initialize(tabsView[i], this);
		}
	}

	public override void Initialize(Node root) {
		base.Initialize(root);
		if (root is not TabContainer tabContainer) {
			GD.PrintErr($"{root.Name} is not a TabContainer!");
			return;
		}
		_tabContainer = tabContainer;
	}

	public void SetCurrentTab(int index) {
		if (index == CurrentIndex)
			return;
		
		TabChanged?.Invoke(index);
		if (index == -1 && _tabContainer.DeselectEnabled) {
			CurrentViewModel?.Blur();
			_tabContainer.SetCurrentTab(index);
			return;
		}
		
		if (index < 0 || Tabs.Count <= index)
			return;
		CurrentViewModel?.Blur();
		_tabContainer.SetCurrentTab(index);
		CurrentViewModel?.Focus();
	}
}