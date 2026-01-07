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
                CommonPopupController.Instance.ShowResult(
                    current == TeamColor.White
                    ? "Black Wins"
                    : "White Wins"
                );
            }
            else
            {
                CommonPopupController.Instance.ShowResult("Draw");
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
