using System.Collections.Generic;
using UnityEngine;

public class MoveGenerator
{
    private readonly IChessRules rules;

    public MoveGenerator(IChessRules rules)
    {
        this.rules = rules;
    }

    public HashSet<Vector2Int> GetLegalMoves(
        Piece piece,
        BoardState board
    )
    {
        HashSet<Vector2Int> moves = new HashSet<Vector2Int>();

        switch (piece.pieceType)
        {
            case PieceType.Pawn:
                GeneratePawnMoves(piece, board, moves);
                break;

            case PieceType.Rook:
                GenerateLinear(piece, board, moves,
                    Vector2Int.up, Vector2Int.down,
                    Vector2Int.left, Vector2Int.right);
                break;

            case PieceType.Bishop:
                GenerateLinear(piece, board, moves,
                    new Vector2Int(1, 1), new Vector2Int(1, -1),
                    new Vector2Int(-1, 1), new Vector2Int(-1, -1));
                break;

            case PieceType.Queen:
                GenerateLinear(piece, board, moves,
                    Vector2Int.up, Vector2Int.down,
                    Vector2Int.left, Vector2Int.right,
                    new Vector2Int(1, 1), new Vector2Int(1, -1),
                    new Vector2Int(-1, 1), new Vector2Int(-1, -1));
                break;

            case PieceType.Knight:
                GenerateKnight(piece, board, moves);
                break;

            case PieceType.King:
                GenerateKing(piece, board, moves);
                break;
        }

        return moves;
    }

    // ================= PAWN =================

    void GeneratePawnMoves(
        Piece piece,
        BoardState board,
        HashSet<Vector2Int> moves
    )
    {
        int dir = piece.teamColor == TeamColor.White ? 1 : -1;
        int startRow = piece.teamColor == TeamColor.White ? 1 : 6;

        int x = piece.boardPosition.x;
        int y = piece.boardPosition.y;

        // Forward
        if (board.GetPieceAt(x, y + dir) == null)
        {
            TryAdd(piece, board, moves, new Vector2Int(x, y + dir));

            if (y == startRow &&
                board.GetPieceAt(x, y + dir * 2) == null)
            {
                TryAdd(piece, board, moves,
                    new Vector2Int(x, y + dir * 2));
            }
        }

        // Capture
        TryCapture(piece, board, moves, x - 1, y + dir);
        TryCapture(piece, board, moves, x + 1, y + dir);
    }

    // ================= LINEAR =================

    void GenerateLinear(
        Piece piece,
        BoardState board,
        HashSet<Vector2Int> moves,
        params Vector2Int[] dirs
    )
    {
        foreach (var dir in dirs)
        {
            Vector2Int pos = piece.boardPosition;

            while (true)
            {
                pos += dir;
                if (!Inside(pos)) break;

                Piece target = board.GetPieceAt(pos.x, pos.y);

                if (target == null)
                {
                    TryAdd(piece, board, moves, pos);
                }
                else
                {
                    if (piece.IsEnemy(target))
                        TryAdd(piece, board, moves, pos);
                    break;
                }
            }
        }
    }

    // ================= KNIGHT =================

    void GenerateKnight(
        Piece piece,
        BoardState board,
        HashSet<Vector2Int> moves
    )
    {
        Vector2Int[] offsets =
        {
            new Vector2Int(1,2), new Vector2Int(2,1),
            new Vector2Int(-1,2), new Vector2Int(-2,1),
            new Vector2Int(1,-2), new Vector2Int(2,-1),
            new Vector2Int(-1,-2), new Vector2Int(-2,-1)
        };

        foreach (var o in offsets)
            TryAdd(piece, board, moves, piece.boardPosition + o);
    }

    // ================= KING =================

    void GenerateKing(Piece piece, BoardState board, HashSet<Vector2Int> moves)
    {
        Vector2Int from = piece.boardPosition;

        // Normal king moves (1 square only)
        for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                Vector2Int target = from + new Vector2Int(dx, dy);

                // Block moving next to enemy king
                if (IsAdjacentToEnemyKing(piece, target, board))
                    continue;

                TryAdd(piece, board, moves, target);
            }

        // Castling (handled explicitly)
        TryAddCastling(piece, board, moves, true);  // King side
        TryAddCastling(piece, board, moves, false); // Queen side
    }
    void TryAddCastling(
        Piece king,
        BoardState board,
        HashSet<Vector2Int> moves,
        bool kingSide
    )
    {
        if (king.hasMoved) return;

        int y = king.boardPosition.y;
        int dir = kingSide ? 1 : -1;

        Vector2Int f1 = king.boardPosition + new Vector2Int(dir, 0);
        Vector2Int f2 = king.boardPosition + new Vector2Int(dir * 2, 0);

        // Squares must be empty
        if (board.GetPieceAt(f1.x, f1.y) != null) return;
        if (board.GetPieceAt(f2.x, f2.y) != null) return;

        // Safety handled by rules.IsMoveSafe
        if (rules.IsMoveSafe(king, f2, board))
            moves.Add(f2);
    }
    bool IsAdjacentToEnemyKing(
        Piece king,
        Vector2Int target,
        BoardState board
    )
    {
        for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                Vector2Int pos = target + new Vector2Int(dx, dy);
                if (pos.x < 0 || pos.x > 7 || pos.y < 0 || pos.y > 7)
                    continue;

                Piece p = board.GetPieceAt(pos.x, pos.y);
                if (p != null &&
                    p.pieceType == PieceType.King &&
                    p.teamColor != king.teamColor)
                    return true;
            }

        return false;
    }



    // ================= HELPERS =================

    void TryAdd(
        Piece piece,
        BoardState board,
        HashSet<Vector2Int> moves,
        Vector2Int pos
    )
    {
        if (!Inside(pos)) return;

        Piece target = board.GetPieceAt(pos.x, pos.y);
        if (target != null && !piece.IsEnemy(target))
            return;

        if (rules.IsMoveSafe(piece, pos, board))
            moves.Add(pos);
    }

    void TryCapture(
        Piece piece,
        BoardState board,
        HashSet<Vector2Int> moves,
        int x, int y
    )
    {
        if (!Inside(new Vector2Int(x, y))) return;

        Piece target = board.GetPieceAt(x, y);
        if (target != null && piece.IsEnemy(target))
            TryAdd(piece, board, moves, new Vector2Int(x, y));
    }

    bool Inside(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;
    }
}
