namespace GameBase.Systems.Debug;

public static class LoggerX
{
	public static void Debug( this Logger log, object message )
	{
#if DEBUG
		Log.Info( $"[{(Host.IsServer ? "SV" : "CL")}] {message}" );
#endif
	}
}
