using UnityEngine;

public class GameContext : Singleton<GameContext>
{
    public IGameMode GameMode { get; private set; }
    public IChessRules Rules { get; private set; }

    public void Init()
    {
        Rules = new ChessRules();
    }

    public void SetGameMode(IGameMode mode)
    {
        GameMode = mode;
    }
}
