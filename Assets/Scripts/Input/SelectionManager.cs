using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    public Piece selectedPiece;
    public Tile selectedTile;

    // Valid moves for currently selected piece
    private HashSet<Vector2Int> validMoves = new HashSet<Vector2Int>();

    #region SELECTION

    public void OnPieceSelected(Piece piece)
    {
        if (!TurnManager.Instance.IsMyTurn(piece.teamColor))
            return;

        ClearSelection();

        selectedPiece = piece;
        selectedTile = BoardManager.Instance.GetTileAt(
            piece.boardPosition.x,
            piece.boardPosition.y
        );

        TileHighlight.Instance.HighlightSelectedTile(selectedTile);
        ShowPossibleMoves(piece);
    }

    public void OnTileSelected(Tile tile)
    {
        if (selectedPiece == null)
            return;

        Vector2Int targetPos = tile.boardPosition;

        if (!validMoves.Contains(targetPos))
            return;

        ExecuteMove(selectedPiece, targetPos);
    }

    #endregion

    #region MOVE GENERATION

    void ShowPossibleMoves(Piece piece)
    {
        switch (piece.pieceType)
        {
            case PieceType.Pawn:
                ShowPawnMoves(piece);
                break;

            case PieceType.Rook:
                ShowLinearMoves(piece,
                    Vector2Int.up, Vector2Int.down,
                    Vector2Int.left, Vector2Int.right);
                break;

            case PieceType.Bishop:
                ShowLinearMoves(piece,
                    new Vector2Int(1, 1), new Vector2Int(1, -1),
                    new Vector2Int(-1, 1), new Vector2Int(-1, -1));
                break;

            case PieceType.Queen:
                ShowLinearMoves(piece,
                    Vector2Int.up, Vector2Int.down,
                    Vector2Int.left, Vector2Int.right,
                    new Vector2Int(1, 1), new Vector2Int(1, -1),
                    new Vector2Int(-1, 1), new Vector2Int(-1, -1));
                break;

            case PieceType.Knight:
                ShowKnightMoves(piece);
                break;

            case PieceType.King:
                ShowKingMoves(piece);
                break;
        }
    }

    #endregion

    #region PAWN LOGIC

    void ShowPawnMoves(Piece piece)
    {
        int dir = piece.teamColor == TeamColor.White ? 1 : -1;
        int startRow = piece.teamColor == TeamColor.White ? 1 : 6;

        int x = piece.boardPosition.x;
        int y = piece.boardPosition.y;

        // 1 step forward
        if (BoardManager.Instance.GetPieceAt(x, y + dir) == null)
        {
            HighlightMove(new Vector2Int(x, y + dir));

            // 2 step forward (first move only)
            if (y == startRow &&
                BoardManager.Instance.GetPieceAt(x, y + dir * 2) == null)
            {
                HighlightMove(new Vector2Int(x, y + dir * 2));
            }
        }

        // Diagonal capture
        TryAddCapture(x - 1, y + dir, piece);
        TryAddCapture(x + 1, y + dir, piece);

        // En Passant
        TryEnPassant(piece);
    }

    void TryEnPassant(Piece pawn)
    {
        int dir = pawn.teamColor == TeamColor.White ? 1 : -1;
        int y = pawn.boardPosition.y;

        int[] sides = { -1, 1 };

        foreach (int side in sides)
        {
            int x = pawn.boardPosition.x + side;
            Piece enemyPawn = BoardManager.Instance.GetPieceAt(x, y);

            if (enemyPawn != null &&
                enemyPawn.pieceType == PieceType.Pawn &&
                enemyPawn.teamColor != pawn.teamColor &&
                enemyPawn.justMovedTwoSteps)
            {
                HighlightCapture(new Vector2Int(x, y + dir));
            }
        }
    }

    #endregion

    #region OTHER PIECES

    void ShowLinearMoves(Piece piece, params Vector2Int[] directions)
    {
        foreach (var dir in directions)
        {
            Vector2Int pos = piece.boardPosition;

            while (true)
            {
                pos += dir;

                if (!IsInsideBoard(pos))
                    break;

                Piece target = BoardManager.Instance.GetPieceAt(pos.x, pos.y);

                if (target == null)
                {
                    HighlightMove(pos);
                }
                else
                {
                    if (piece.IsEnemy(target))
                        HighlightCapture(pos);
                    break;
                }
            }
        }
    }

    void ShowKnightMoves(Piece piece)
    {
        Vector2Int[] moves =
        {
            new Vector2Int(1,2), new Vector2Int(2,1),
            new Vector2Int(-1,2), new Vector2Int(-2,1),
            new Vector2Int(1,-2), new Vector2Int(2,-1),
            new Vector2Int(-1,-2), new Vector2Int(-2,-1)
        };

        foreach (var move in moves)
        {
            TryAddMoveOrCapture(piece.boardPosition + move, piece);
        }
    }

    void ShowKingMoves(Piece piece)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                TryAddMoveOrCapture(piece.boardPosition + new Vector2Int(x, y), piece);
            }
        }

        TryCastling(piece);
    }

    #endregion

    #region EXECUTE MOVE

    void ExecuteMove(Piece piece, Vector2Int targetPos)
    {
        Vector2Int startPos = piece.boardPosition;

        // En Passant capture
        if (piece.pieceType == PieceType.Pawn &&
            BoardManager.Instance.GetPieceAt(targetPos.x, targetPos.y) == null)
        {
            int dir = piece.teamColor == TeamColor.White ? -1 : 1;
            Piece enPassantPawn = BoardManager.Instance.GetPieceAt(
                targetPos.x, targetPos.y + dir);

            if (enPassantPawn != null &&
                enPassantPawn.pieceType == PieceType.Pawn &&
                enPassantPawn.justMovedTwoSteps)
            {
                StartCoroutine(AnimateCapture(enPassantPawn));
                BoardManager.Instance.SetPieceAt(targetPos.x, targetPos.y + dir, null);
            }
        }

        // Normal capture
        Piece target = BoardManager.Instance.GetPieceAt(targetPos.x, targetPos.y);
        if (target != null)
        {
            UIManager.Instance.AddCapturedPiece(target);
            StartCoroutine(AnimateCapture(target));
        }

        // Castling move
        if (piece.pieceType == PieceType.King &&
            Mathf.Abs(startPos.x - targetPos.x) == 2)
        {
            int rookFromX = targetPos.x > startPos.x ? 7 : 0;
            int rookToX = targetPos.x > startPos.x ? targetPos.x - 1 : targetPos.x + 1;

            Piece rook = BoardManager.Instance.GetPieceAt(rookFromX, startPos.y);
            if (rook != null)
            {
                BoardManager.Instance.SetPieceAt(rookFromX, startPos.y, null);
                BoardManager.Instance.SetPieceAt(rookToX, startPos.y, rook);

                rook.transform.SetParent(
                    BoardManager.Instance.GetTileAt(rookToX, startPos.y).transform
                );
                rook.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                rook.boardPosition = new Vector2Int(rookToX, startPos.y);
                rook.hasMoved = true;
            }
        }

        // Update board
        BoardManager.Instance.SetPieceAt(startPos.x, startPos.y, null);
        BoardManager.Instance.SetPieceAt(targetPos.x, targetPos.y, piece);

        // Move UI (Animated)
        StartCoroutine(AnimateMove(piece, targetPos));

        piece.boardPosition = targetPos;
        piece.hasMoved = true;

        // Pawn promotion
        if (piece.pieceType == PieceType.Pawn &&
            ((piece.teamColor == TeamColor.White && targetPos.y == 7) ||
             (piece.teamColor == TeamColor.Black && targetPos.y == 0)))
        {
            PromotePawn(piece);
        }

        // Reset en-passant flags
        foreach (var p in FindObjectsByType<Piece>(FindObjectsSortMode.None))
        {
            if (p.pieceType == PieceType.Pawn)
                p.justMovedTwoSteps = false;
        }

        TurnManager.Instance.SwitchTurn();
        TurnManager.Instance.EvaluateGameState();
        ClearSelection();
    }

    IEnumerator AnimateMove(Piece piece, Vector2Int targetPos)
    {
        RectTransform pieceRT = piece.GetComponent<RectTransform>();
        RectTransform targetTileRT =
            BoardManager.Instance.GetTileAt(targetPos.x, targetPos.y)
            .GetComponent<RectTransform>();

        // Animate move
        yield return StartCoroutine(
            PieceAnimator.MoveTo(pieceRT, targetTileRT)
        );

        // Re-parent AFTER animation
        piece.transform.SetParent(targetTileRT);
        pieceRT.anchoredPosition = Vector2.zero;
    }

    IEnumerator AnimateCapture(Piece target)
    {
        RectTransform rt = target.GetComponent<RectTransform>();
        yield return StartCoroutine(PieceAnimator.CaptureEffect(rt));
        Destroy(target.gameObject);
    }

    void PromotePawn(Piece pawn)
    {
        pawn.pieceType = PieceType.Queen;
        pawn.image.sprite = pawn.teamColor == TeamColor.White
            ? PieceSpawner.Instance.whiteQueen
            : PieceSpawner.Instance.blackQueen;
    }

    #endregion

    #region HELPERS

    bool IsInsideBoard(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;
    }

    void TryAddMoveOrCapture(Vector2Int pos, Piece piece)
    {
        if (!IsInsideBoard(pos)) return;

        Piece target = BoardManager.Instance.GetPieceAt(pos.x, pos.y);

        if (target == null)
            HighlightMove(pos);
        else if (piece.IsEnemy(target))
            HighlightCapture(pos);
    }

    void TryAddCapture(int x, int y, Piece piece)
    {
        if (!IsInsideBoard(new Vector2Int(x, y))) return;

        Piece target = BoardManager.Instance.GetPieceAt(x, y);
        if (target != null && piece.IsEnemy(target))
            HighlightCapture(new Vector2Int(x, y));
    }

    void HighlightMove(Vector2Int pos)
    {
        if (selectedPiece == null || !IsMoveSafe(selectedPiece, pos)) return;

        validMoves.Add(pos);
        TileHighlight.Instance.HighlightMoveTile(
            BoardManager.Instance.GetTileAt(pos.x, pos.y)
        );
    }

    void HighlightCapture(Vector2Int pos)
    {
        if (selectedPiece == null || !IsMoveSafe(selectedPiece, pos)) return;

        validMoves.Add(pos);
        TileHighlight.Instance.HighlightCaptureTile(
            BoardManager.Instance.GetTileAt(pos.x, pos.y)
        );
    }

    bool IsSquareUnderAttack(Vector2Int square, TeamColor byTeam)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Piece p = BoardManager.Instance.GetPieceAt(x, y);
                if (p == null || p.teamColor != byTeam)
                    continue;

                if (CanPieceAttackSquare(p, square))
                    return true;
            }
        }
        return false;
    }

    bool CanPieceAttackSquare(Piece piece, Vector2Int target)
    {
        Vector2Int from = piece.boardPosition;

        switch (piece.pieceType)
        {
            case PieceType.Pawn:
                int dir = piece.teamColor == TeamColor.White ? 1 : -1;
                return target == from + new Vector2Int(1, dir) ||
                       target == from + new Vector2Int(-1, dir);

            case PieceType.Knight:
                Vector2Int[] knightMoves =
                {
                    new Vector2Int(1,2), new Vector2Int(2,1),
                    new Vector2Int(-1,2), new Vector2Int(-2,1),
                    new Vector2Int(1,-2), new Vector2Int(2,-1),
                    new Vector2Int(-1,-2), new Vector2Int(-2,-1)
                };
                foreach (var m in knightMoves)
                    if (from + m == target) return true;
                return false;

            case PieceType.Bishop:
                return IsSlidingAttack(from, target,
                    new Vector2Int(1, 1), new Vector2Int(1, -1),
                    new Vector2Int(-1, 1), new Vector2Int(-1, -1));

            case PieceType.Rook:
                return IsSlidingAttack(from, target,
                    Vector2Int.up, Vector2Int.down,
                    Vector2Int.left, Vector2Int.right);

            case PieceType.Queen:
                return IsSlidingAttack(from, target,
                    Vector2Int.up, Vector2Int.down,
                    Vector2Int.left, Vector2Int.right,
                    new Vector2Int(1, 1), new Vector2Int(1, -1),
                    new Vector2Int(-1, 1), new Vector2Int(-1, -1));

            case PieceType.King:
                return Mathf.Abs(from.x - target.x) <= 1 &&
                       Mathf.Abs(from.y - target.y) <= 1;
        }

        return false;
    }

    bool IsSlidingAttack(Vector2Int from, Vector2Int target, params Vector2Int[] dirs)
    {
        foreach (var dir in dirs)
        {
            Vector2Int pos = from;
            while (true)
            {
                pos += dir;
                if (!IsInsideBoard(pos)) break;

                Piece block = BoardManager.Instance.GetPieceAt(pos.x, pos.y);
                if (pos == target) return true;
                if (block != null) break;
            }
        }
        return false;
    }

    bool IsMoveSafe(Piece piece, Vector2Int target)
    {
        Vector2Int start = piece.boardPosition;
        Piece captured = BoardManager.Instance.GetPieceAt(target.x, target.y);

        // simulate
        BoardManager.Instance.SetPieceAt(start.x, start.y, null);
        BoardManager.Instance.SetPieceAt(target.x, target.y, piece);
        piece.boardPosition = target;

        Vector2Int kingPos = piece.pieceType == PieceType.King
            ? target
            : BoardManager.Instance.GetKingPosition(piece.teamColor);

        bool inCheck = IsSquareUnderAttack(
            kingPos,
            piece.teamColor == TeamColor.White ? TeamColor.Black : TeamColor.White
        );

        // revert
        piece.boardPosition = start;
        BoardManager.Instance.SetPieceAt(start.x, start.y, piece);
        BoardManager.Instance.SetPieceAt(target.x, target.y, captured);

        return !inCheck;
    }

    public bool HasAnyLegalMove(TeamColor team)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Piece piece = BoardManager.Instance.GetPieceAt(x, y);
                if (piece == null || piece.teamColor != team)
                    continue;

                selectedPiece = piece;
                validMoves.Clear();

                ShowPossibleMoves(piece);

                if (validMoves.Count > 0)
                {
                    ClearSelection();
                    return true;
                }
            }
        }

        ClearSelection();
        return false;
    }

    public bool IsKingInCheck(TeamColor team)
    {
        Vector2Int kingPos = BoardManager.Instance.GetKingPosition(team);
        TeamColor enemy = team == TeamColor.White ? TeamColor.Black : TeamColor.White;

        return IsSquareUnderAttack(kingPos, enemy);
    }

    void TryCastling(Piece king)
    {
        if (king.hasMoved) return;
        if (IsKingInCheck(king.teamColor)) return;

        int y = king.boardPosition.y;

        TryCastleSide(king, 7, y, 5, 6); // King side
        TryCastleSide(king, 0, y, 3, 2); // Queen side
    }

    void TryCastleSide(Piece king, int rookX, int y, int passX, int targetX)
    {
        Piece rook = BoardManager.Instance.GetPieceAt(rookX, y);
        if (rook == null || rook.hasMoved || rook.pieceType != PieceType.Rook)
            return;

        int dir = targetX > king.boardPosition.x ? 1 : -1;

        // Path clear
        for (int x = king.boardPosition.x + dir; x != rookX; x += dir)
            if (BoardManager.Instance.GetPieceAt(x, y) != null)
                return;

        // Squares not under attack
        for (int x = king.boardPosition.x; x != targetX + dir; x += dir)
        {
            if (IsSquareUnderAttack(new Vector2Int(x, y),
                king.teamColor == TeamColor.White ? TeamColor.Black : TeamColor.White))
                return;
        }

        HighlightMove(new Vector2Int(targetX, y));
    }

    void ClearSelection()
    {
        TileHighlight.Instance.ClearAll();
        validMoves.Clear();
        selectedPiece = null;
        selectedTile = null;
    }

    public void ClearSelectionExternally()
    {
        ClearSelection();
    }

    #endregion
}
