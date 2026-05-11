using UnityEngine;

public class EnPassantRule
{
    public void Apply(MoveData move, BoardState board)
    {
        if (move.piece.pieceType != PieceType.Pawn)
            return;

        if (board.GetPieceAt(move.to.x, move.to.y) != null)
            return;

        int dir = move.piece.teamColor == TeamColor.White ? -1 : 1;

        Piece pawn =
            board.GetPieceAt(move.to.x, move.to.y + dir);

        if (pawn == null ||
            pawn.pieceType != PieceType.Pawn ||
            !pawn.justMovedTwoSteps)
            return;

        move.isEnPassant = true;
        move.isCapture = true;
        move.capturedPiecePos =
            new Vector2Int(move.to.x, move.to.y + dir);
    }
}
