using GameBase.Systems.CloudAssets;
using GameBase.Systems.Debug;

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
			EnableShadowCasting = true;
			EnableShadowInFirstPerson = true;
		}

		public override void Simulate( Client cl )
		{
			base.Simulate( cl );

			if ( Input.Pressed( InputButton.PrimaryAttack ) )
			{
				if ( IsServer )
				{
					var prop = new Prop();
					prop.Position = EyePosition + EyeRotation.Forward * 100;

					var model = CloudAsset.LoadModel( $"baik.woodenbarrel", this );
					prop.Model = model;

					var material = CloudAsset.LoadMaterial( $"mungus.garry2", this );
					prop.SetMaterialOverride( material );

					var sound = CloudAsset.LoadSound( $"trend.spectrometer", this );
					Sound.FromEntity( sound, this );
				}
			}
		}
	}
}
