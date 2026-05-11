using UnityEngine;

public class CheckEvaluator
{
    public bool IsKingInCheck(TeamColor team, BoardState board)
    {
        Vector2Int kingPos = board.GetKingPosition(team);
        TeamColor enemy = Opponent(team);
        return IsSquareUnderAttack(kingPos, enemy, board);
    }

    public bool IsSquareUnderAttack(
        Vector2Int square,
        TeamColor byTeam,
        BoardState board
    )
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Piece p = board.GetPieceAt(x, y);
                if (p == null || p.teamColor != byTeam)
                    continue;

                if (CanPieceAttackSquare(p, square, board))
                    return true;
            }
        }
        return false;
    }

    public bool IsMoveSafe(
        Piece piece,
        Vector2Int target,
        BoardState board
    )
    {
        Vector2Int from = piece.boardPosition;
        Piece captured = board.GetPieceAt(target.x, target.y);

        board.SetPieceAt(from.x, from.y, null);
        board.SetPieceAt(target.x, target.y, piece);
        piece.boardPosition = target;

        Vector2Int kingPos =
            piece.pieceType == PieceType.King
            ? target
            : board.GetKingPosition(piece.teamColor);

        bool inCheck = IsSquareUnderAttack(
            kingPos,
            Opponent(piece.teamColor),
            board
        );

        piece.boardPosition = from;
        board.SetPieceAt(from.x, from.y, piece);
        board.SetPieceAt(target.x, target.y, captured);

        return !inCheck;
    }

    // ================= INTERNAL =================

    bool CanPieceAttackSquare(
        Piece piece,
        Vector2Int target,
        BoardState board
    )
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
                return SlidingAttack(from, target, board,
                    new Vector2Int(1,1), new Vector2Int(1,-1),
                    new Vector2Int(-1,1), new Vector2Int(-1,-1));

            case PieceType.Rook:
                return SlidingAttack(from, target, board,
                    Vector2Int.up, Vector2Int.down,
                    Vector2Int.left, Vector2Int.right);

            case PieceType.Queen:
                return SlidingAttack(from, target, board,
                    Vector2Int.up, Vector2Int.down,
                    Vector2Int.left, Vector2Int.right,
                    new Vector2Int(1,1), new Vector2Int(1,-1),
                    new Vector2Int(-1,1), new Vector2Int(-1,-1));

            case PieceType.King:
                return Mathf.Abs(from.x - target.x) <= 1 &&
                       Mathf.Abs(from.y - target.y) <= 1;
        }
        return false;
    }

    bool SlidingAttack(
        Vector2Int from,
        Vector2Int target,
        BoardState board,
        params Vector2Int[] dirs
    )
    {
        foreach (var dir in dirs)
        {
            Vector2Int pos = from;
            while (true)
            {
                pos += dir;
                if (!Inside(pos)) break;

                Piece block = board.GetPieceAt(pos.x, pos.y);
                if (pos == target) return true;
                if (block != null) break;
            }
        }
        return false;
    }

    bool Inside(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;
    }

    TeamColor Opponent(TeamColor t)
    {
        return t == TeamColor.White ? TeamColor.Black : TeamColor.White;
    }
}
