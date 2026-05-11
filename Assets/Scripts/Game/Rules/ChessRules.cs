using UnityEngine;

public class ChessRules : IChessRules
{
    private readonly CastlingRule castlingRule;
    private readonly EnPassantRule enPassantRule;
    private readonly PromotionRule promotionRule;

    private readonly CheckEvaluator checkEvaluator;

    public ChessRules()
    {
        checkEvaluator = new CheckEvaluator();
        castlingRule = new CastlingRule(checkEvaluator);
        enPassantRule = new EnPassantRule();
        promotionRule = new PromotionRule();
    }


    public bool IsSquareUnderAttack(
        Vector2Int square,
        TeamColor byTeam,
        BoardState board
    )
    {
        return checkEvaluator.IsSquareUnderAttack(square, byTeam, board);
    }

    public bool IsMoveSafe(
        Piece piece,
        Vector2Int target,
        BoardState board
    )
    {
        return checkEvaluator.IsMoveSafe(piece, target, board);
    }

    public bool IsKingInCheck(
        TeamColor team,
        BoardState board
    )
    {
        return checkEvaluator.IsKingInCheck(team, board);
    }
    public MoveData CreateMove(
    Piece piece,
    Vector2Int target,
    BoardState board
    )
    {
        MoveData move = new MoveData
        {
            piece = piece,
            from = piece.boardPosition,
            to = target
        };

        Piece targetPiece = board.GetPieceAt(target.x, target.y);

        // Normal capture
        if (targetPiece != null && piece.IsEnemy(targetPiece))
        {
            move.isCapture = true;
            move.capturedPiecePos = target;
        }

        // En Passant
        enPassantRule.Apply(move, board);

        // Castling
        castlingRule.Apply(move, board);

        // Promotion
        promotionRule.Apply(move, board);

        return move;
    }

}
