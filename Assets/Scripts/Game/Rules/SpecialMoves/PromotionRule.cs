public class PromotionRule
{
    public void Apply(MoveData move, BoardState board)
    {
        if (move.piece.pieceType != PieceType.Pawn)
            return;

        if (move.to.y == 0 || move.to.y == 7)
        {
            move.isPromotion = true;
        }
    }
}
