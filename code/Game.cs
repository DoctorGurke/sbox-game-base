namespace GameBase;

public partial class MyGame : Game
{
	public override void Spawn()
	{
		base.Spawn();

		Transmit = TransmitType.Always;


		// do init stuff here, not in ctor
	}

	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		// Create a pawn for this client to play with
		var pawn = new Player();
		client.Pawn = pawn;
		pawn.Respawn();
	}
}
