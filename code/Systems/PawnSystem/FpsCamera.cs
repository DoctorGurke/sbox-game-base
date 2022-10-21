namespace GameBase.Systems.PawnSystem;

public class FpsCamera : CameraMode
{
	Vector3 lastPos;
	bool resetLerp;

	public override void Activated()
	{
		var pawn = Local.Pawn;
		if ( pawn == null ) return;

		Position = pawn.EyePosition;
		Rotation = pawn.EyeRotation;

		lastPos = Position;
	}

	public override void Update()
	{
		var pawn = Local.Pawn;
		if ( pawn == null ) return;

		var eyePos = pawn.EyePosition;
		if ( resetLerp )
		{
			Position = eyePos;
			resetLerp = false;
		}
		else
		{
			Position = Vector3.Lerp( eyePos.WithZ( lastPos.z ), eyePos, 20.0f * Time.Delta );
		}

		Rotation = pawn.EyeRotation;

		Viewer = pawn;
		lastPos = Position;
	}

	public void ResetInterpolation()
	{
		resetLerp = true;
	}
}
