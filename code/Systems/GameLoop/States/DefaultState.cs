using GameBase.Systems.Debug;

namespace GameBase.Systems.GameLoop.States;

public class DefaultState : GameState
{
	public override void OnStart()
	{
		Log.Debug( $"state start" );
	}

	public override void OnStop()
	{
		Log.Debug( $"state stop" );
	}

	public override void OnRestart()
	{
		Log.Debug( $"state restart" );
	}

	public override void OnReset()
	{
		Log.Debug( $"state reset" );
	}

	public override void OnInit()
	{
		Log.Debug( $"state init" );
	}
}
