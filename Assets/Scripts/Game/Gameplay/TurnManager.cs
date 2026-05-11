using UnityEngine;

public class TurnManager : Singleton<TurnManager>
{
    public TeamColor currentTurn = TeamColor.White;

    private IChessRules rules;

    protected override void Awake()
    {
        base.Awake();
        rules = new ChessRules();
    }

    public bool IsMyTurn(TeamColor team)
    {
        return team == currentTurn;
    }

    public void SwitchTurn()
    {
        currentTurn = currentTurn == TeamColor.White
            ? TeamColor.Black
            : TeamColor.White;

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
        bool inCheck = rules.IsKingInCheck(
            current,
            BoardManager.Instance.BoardState
        );

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
