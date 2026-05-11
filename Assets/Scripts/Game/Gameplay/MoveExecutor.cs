using System.Collections;
using UnityEngine;

public class MoveExecutor : Singleton<MoveExecutor>
{
    public void ExecuteMove(Piece piece, Vector2Int target, bool isRemote = false)
    {
        MoveData move =
            GameContext.Instance.Rules.CreateMove(
                    piece,
                    target,
                    BoardManager.Instance.BoardState
                );

        StartCoroutine(ExecuteRoutine(move));
    }


    IEnumerator ExecuteRoutine(MoveData move)
    {
        BoardManager board = BoardManager.Instance;
        BoardState state = board.BoardState;

        // Capture
        if (move.isCapture)
        {
            Piece captured =
                state.GetPieceAt(
                    move.capturedPiecePos.x,
                    move.capturedPiecePos.y);

            if (captured != null)
            {
                UIManager.Instance.AddCapturedPiece(captured);
                yield return Capture(captured);
                state.SetPieceAt(
                    move.capturedPiecePos.x,
                    move.capturedPiecePos.y,
                    null);
            }
        }

        // Castling rook move
        if (move.isCastling)
        {
            Piece rook =
                state.GetPieceAt(move.rookFrom.x, move.rookFrom.y);

            state.SetPieceAt(move.rookFrom.x, move.rookFrom.y, null);
            state.SetPieceAt(move.rookTo.x, move.rookTo.y, rook);

            Tile rookTile =
                board.GetTileAt(move.rookTo.x, move.rookTo.y);

            rook.transform.SetParent(rookTile.transform);
            rook.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            rook.boardPosition = move.rookTo;
            rook.hasMoved = true;
        }

        // Move piece
        state.SetPieceAt(move.from.x, move.from.y, null);
        state.SetPieceAt(move.to.x, move.to.y, move.piece);

        yield return AnimateMove(move.piece, move.to);

        move.piece.boardPosition = move.to;
        move.piece.hasMoved = true;

        // Reset all pawn en-passant flags
        foreach (Piece p in FindObjectsByType<Piece>(FindObjectsSortMode.None))
        {
            if (p.pieceType == PieceType.Pawn)
                p.justMovedTwoSteps = false;
        }

        // Mark pawn double-step
        if (move.piece.pieceType == PieceType.Pawn &&
            Mathf.Abs(move.from.y - move.to.y) == 2)
        {
            move.piece.justMovedTwoSteps = true;
        }


        // Promotion
        if (move.isPromotion)
        {
            PromotePawn(move.piece);
        }

        TileHighlight.Instance.HighlightLastMove(
            board.GetTileAt(move.from.x, move.from.y),
            board.GetTileAt(move.to.x, move.to.y));

        GameContext.Instance.GameMode
            .OnMoveExecuted(move.from, move.to);

        if (!(GameContext.Instance.GameMode is OnlineGameMode))
        {
            TurnManager.Instance?.EvaluateGameState();
        }

    }

    // ================= HELPERS =================

    IEnumerator AnimateMove(Piece piece, Vector2Int target)
    {
        RectTransform rt = piece.GetComponent<RectTransform>();
        RectTransform targetRT =
            BoardManager.Instance
                .GetTileAt(target.x, target.y)
                .GetComponent<RectTransform>();

        yield return PieceAnimator.MoveTo(rt, targetRT);

        piece.transform.SetParent(targetRT);
        rt.anchoredPosition = Vector2.zero;
    }

    IEnumerator Capture(Piece piece)
    {
        yield return PieceAnimator.CaptureEffect(
            piece.GetComponent<RectTransform>());
        Destroy(piece.gameObject);
    }

    void HandleCastling(
        Piece king,
        Vector2Int target,
        BoardState state
    )
    {
        int y = king.boardPosition.y;
        int rookFromX = target.x > king.boardPosition.x ? 7 : 0;
        int rookToX = target.x > king.boardPosition.x
            ? target.x - 1
            : target.x + 1;

        Piece rook = state.GetPieceAt(rookFromX, y);
        if (rook == null) return;

        state.SetPieceAt(rookFromX, y, null);
        state.SetPieceAt(rookToX, y, rook);

        Tile rookTile =
            BoardManager.Instance.GetTileAt(rookToX, y);

        rook.transform.SetParent(rookTile.transform);
        rook.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        rook.boardPosition = new Vector2Int(rookToX, y);
        rook.hasMoved = true;
    }

    void PromotePawn(Piece pawn)
    {
        pawn.pieceType = PieceType.Queen;
        pawn.image.sprite =
            pawn.teamColor == TeamColor.White
            ? PieceSpawner.Instance.whiteQueen
            : PieceSpawner.Instance.blackQueen;
    }
}
