using System;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace PingPongPang.Scripts.Utilities;

public class Debouncer {
	private readonly Timer _timer;

	public Debouncer() {
		_timer = new Timer();
		_timer.Elapsed += (s, e) => Execute();
	}

	private Action _action;

	public void Debounce(Action action, int delay = 300) {
		_timer.Interval = delay;
		_action = action;
		_timer.Stop();
		_timer.Start();
	}

	private void Execute() {
		_timer.Stop();
		_action?.Invoke();
	}
}