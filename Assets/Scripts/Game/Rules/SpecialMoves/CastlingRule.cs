using UnityEngine;

public class CastlingRule
{
    private readonly CheckEvaluator check;

    public CastlingRule(CheckEvaluator evaluator)
    {
        check = evaluator;
    }

    public void Apply(MoveData move, BoardState board)
    {
        if (move.piece.pieceType != PieceType.King)
            return;

        if (move.piece.hasMoved)
            return;

        int dx = move.to.x - move.from.x;
        if (Mathf.Abs(dx) != 2)
            return;

        TeamColor team = move.piece.teamColor;
        int y = move.from.y;
        bool kingSide = dx > 0;

        int rookX = kingSide ? 7 : 0;
        Piece rook = board.GetPieceAt(rookX, y);

        if (rook == null ||
            rook.pieceType != PieceType.Rook ||
            rook.teamColor != team ||
            rook.hasMoved)
            return;

        // King must not be in check
        if (check.IsKingInCheck(team, board))
            return;

        int step = kingSide ? 1 : -1;
        int x = move.from.x;

        // Squares between king & rook
        for (int i = 1; i <= 2; i++)
        {
            Vector2Int pos = new Vector2Int(x + step * i, y);

            if (board.GetPieceAt(pos.x, pos.y) != null)
                return;

            if (check.IsSquareUnderAttack(pos,
                team == TeamColor.White ? TeamColor.Black : TeamColor.White,
                board))
                return;
        }

        move.isCastling = true;
        move.rookFrom = new Vector2Int(rookX, y);
        move.rookTo = new Vector2Int(move.to.x - step, y);
    }
}
