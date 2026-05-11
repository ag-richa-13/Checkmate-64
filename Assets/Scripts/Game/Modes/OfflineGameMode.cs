using UnityEngine;

public class OfflineGameMode : IGameMode
{
    public bool IsMyTurn(TeamColor team)
    {
        return TurnManager.Instance.IsMyTurn(team);
    }

    public void OnMoveExecuted(Vector2Int from, Vector2Int to)
    {
        TurnManager.Instance.SwitchTurn();
    }
}
