using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    public TeamColor currentTurn = TeamColor.White;

    public bool IsMyTurn(TeamColor team)
    {
        return team == currentTurn;
    }

    public void SwitchTurn()
    {
        currentTurn = currentTurn == TeamColor.White
            ? TeamColor.Black
            : TeamColor.White;

        Debug.Log($"Turn: {currentTurn}");
        UIManager.Instance.UpdateTurnUI(currentTurn);
    }

    public void ResetTurn()
    {
        currentTurn = TeamColor.White;
        UIManager.Instance.UpdateTurnUI(currentTurn);
    }

    public void EvaluateGameState()
    {
        TeamColor current = currentTurn;
        SelectionManager sm = SelectionManager.Instance;

        bool hasMoves = sm.HasAnyLegalMove(current);
        bool inCheck = sm.IsKingInCheck(current);

        if (!hasMoves)
        {
            if (inCheck)
            {
                UIManager.Instance.ShowCheckmate();
                UIManager.Instance.ShowGameEnd(
                    current == TeamColor.White
                    ? "Black Wins"
                    : "White Wins"
                );
            }
            else
            {
                UIManager.Instance.ShowStalemate();
                UIManager.Instance.ShowGameEnd("Draw");
            }
        }
        else if (inCheck)
        {
            UIManager.Instance.ShowCheck();
        }
        else
        {
            UIManager.Instance.ClearStatus();
        }
    }
}
