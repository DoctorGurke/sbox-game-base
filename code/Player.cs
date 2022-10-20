namespace GameBase
{
	public class Player : Pawn
	{
		public override void Respawn()
		{
			SetModel( "models/citizen/citizen.vmdl" );

			base.Respawn();

			Controller = new WalkController();
			Camera = new FpsCamera();
			Animator = new StandardPlayerAnimator();

			EnableDrawing = true;
			EnableHideInFirstPerson = true;
		}
	}
}
