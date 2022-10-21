namespace GameBase.Systems.GameLoop.States;

public interface IGameState
{
	public void Start();
	public void Stop();
	public void Toggle();
	public void Restart();
	public void Reset();
	public void Kill();
}
