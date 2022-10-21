namespace GameBase.Systems.GameLoop.States;

public abstract partial class GameState : Entity, IGameState
{
	public override void Spawn()
	{
		base.Spawn();

		Transmit = TransmitType.Always;
	}

	/// <summary>
	/// Whether this state is currently running or not.
	/// </summary>
	[Net]
	public bool IsRunning { get; set; }

	/// <summary>
	/// If this state was started before. Gets reset via Reset().
	/// </summary>
	[Net]
	private bool _Init { get; set; }

	/// <summary>
	/// If this state was killed. Try to catch any edge case with attempting to process logic in this state if it is marked as killed.
	/// </summary>
	[Net]
	private bool _Killed { get; set; }

	/// <summary>
	/// Used to measure segments when the game is running.
	/// </summary>
	[Net]
	private TimeSince _TimeSince { get; set; }

	/// <summary>
	/// Time this state has spent running.
	/// </summary>
	[Net]
	private float _Time { get; set; }

	/// <summary>
	/// Gets the time the state has been running.
	/// </summary>
	public float Time
	{
		get
		{
			// paused, time is accounted for via Stop() or Reset()
			if ( !IsRunning )
				return _Time;

			// account for offset since this is during running
			return _Time + _TimeSince;
		}
	}

	[Net]
	public TimeSince TimeSincePaused { get; set; }

	/// <summary>
	/// Start or continue the state, initializes if state is in reset or uninitialized mode. Starts time measurement.
	/// </summary>
	public void Start()
	{
		Host.AssertServer();

		if ( _Killed )
		{
			Log.Error( $"Killed State {this} tried to start!" );
			return;
		}

		IsRunning = true;
		_TimeSince = 0;

		// first time starting, reset all
		if ( !_Init )
		{
			Init();
		}

		OnStart();
		OnClientStart( To.Everyone );
	}

	/// <summary>
	/// Pauses the state. Stops time measurement.
	/// </summary>
	public void Stop()
	{
		Host.AssertServer();

		if ( _Killed )
		{
			Log.Error( $"Killed State {this} tried to stop!" );
			return;
		}

		if ( !IsRunning )
			return;

		IsRunning = false;
		TimeSincePaused = 0;

		_Time += _TimeSince;

		OnStop();
		OnClientStop( To.Everyone );
	}

	public void Toggle()
	{
		if ( IsRunning )
			Stop();
		else
			Start();
	}

	/// <summary>
	/// Restart the state. Resets time measurement.
	/// </summary>
	public void Restart()
	{
		Host.AssertServer();

		if ( _Killed )
		{
			Log.Error( $"Killed State {this} tried to restart!" );
			return;
		}

		IsRunning = true;
		_Time = 0;
		_TimeSince = 0;

		OnRestart();
		OnClientRestart( To.Everyone );
	}

	/// <summary>
	/// Resets the state. Stops time measurement and resets running.
	/// </summary>
	public void Reset()
	{
		Host.AssertServer();

		if ( _Killed )
		{
			Log.Error( $"Killed State {this} tried to reset!" );
			return;
		}

		IsRunning = false;
		_Init = false;
		_Time = 0;
		_TimeSince = 0;

		OnReset();
		OnClientReset( To.Everyone );
	}

	public void Kill()
	{
		Host.AssertServer();

		if ( _Killed )
		{
			Log.Error( $"Tried killing already killed state {this}!" );
			return;
		}

		_Killed = true;
	}

	private void Init()
	{
		Host.AssertServer();

		if ( _Killed )
		{
			Log.Error( $"Killed State {this} tried to init!" );
			return;
		}

		_Init = true;
		_Time = 0;
		_TimeSince = 0;

		OnInit();
		OnClientInit( To.Everyone );
	}

	[Event.Tick]
	private void Tick()
	{
		if ( _Killed )
		{
			Log.Error( $"Killed State {this} tried to think!" );
			return;
		}

		if ( !IsRunning )
			return;

		// only think while state is running
		OnThink();
		OnClientThink( To.Everyone );
	}

	[ClientRpc]
	private void OnClientStart()
	{
		Host.AssertClient();
		OnStart();
	}
	public virtual void OnStart() { }

	[ClientRpc]
	private void OnClientStop()
	{
		Host.AssertClient();
		OnStop();
	}
	public virtual void OnStop() { }

	[ClientRpc]
	private void OnClientRestart()
	{
		Host.AssertClient();
		OnRestart();
	}
	public virtual void OnRestart() { }

	[ClientRpc]
	private void OnClientReset()
	{
		Host.AssertClient();
		OnReset();
	}
	public virtual void OnReset() { }

	[ClientRpc]
	private void OnClientInit()
	{
		Host.AssertClient();
		OnInit();
	}
	public virtual void OnInit() { }

	[ClientRpc]
	private void OnClientThink()
	{
		Host.AssertClient();
		OnThink();
	}
	public virtual void OnThink() { }
}
