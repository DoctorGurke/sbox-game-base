using GameBase.Systems.GameLoop.States;

namespace GameBase;

public partial class MyGame
{
	public GameState? State
	{
		get
		{
			return _State;
		}
		set
		{
			Host.AssertServer();
			_State?.Kill();
			_State = null;
			_State = value;
		}
	}

	[Net]
	private GameState? _State { get; set; }
}
