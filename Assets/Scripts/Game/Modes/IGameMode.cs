using UnityEngine;

public interface IGameMode
{
    bool IsMyTurn(TeamColor team);
    void OnMoveExecuted(Vector2Int from, Vector2Int to);
}
