using GameBase.Systems.GameLoop.States;

namespace GameBase;

public partial class MyGame
{
	[Net]
	public GameState? State { get; set; }
}
