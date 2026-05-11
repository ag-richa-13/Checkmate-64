using UnityEngine;

public interface IChessRules
{
    bool IsSquareUnderAttack(
        Vector2Int square,
        TeamColor byTeam,
        BoardState board
    );

    bool IsMoveSafe(
        Piece piece,
        Vector2Int target,
        BoardState board
    );

    bool IsKingInCheck(
        TeamColor team,
        BoardState board
    );
    MoveData CreateMove(
            Piece piece,
            Vector2Int target,
            BoardState board
        );
}
