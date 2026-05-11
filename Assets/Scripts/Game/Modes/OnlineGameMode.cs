using UnityEngine;

public class OnlineGameMode : IGameMode
{
    public bool IsMyTurn(TeamColor team)
    {
        if (!OnlineTurnManager.Instance.IsMyTurn())
            return false;

        TeamColor myColor =
            PhotonPlayerData.LocalSide == PlayerSide.White
                ? TeamColor.White
                : TeamColor.Black;

        return team == myColor;
    }

    public void OnMoveExecuted(Vector2Int from, Vector2Int to)
    {
        PhotonMoveSync.Instance.SendMove(from, to);
    }
}
