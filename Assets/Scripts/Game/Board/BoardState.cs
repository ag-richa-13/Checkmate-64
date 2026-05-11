using UnityEngine;
public class BoardState
{
    private Piece[,] pieces = new Piece[8, 8];

    public Piece GetPieceAt(int x, int y)
    {
        if (x < 0 || x > 7 || y < 0 || y > 7) return null;
        return pieces[x, y];
    }

    public void SetPieceAt(int x, int y, Piece piece)
    {
        if (x < 0 || x > 7 || y < 0 || y > 7) return;
        pieces[x, y] = piece;
    }

    public Vector2Int GetKingPosition(TeamColor team)
    {
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
            {
                Piece p = pieces[x, y];
                if (p != null &&
                    p.pieceType == PieceType.King &&
                    p.teamColor == team)
                    return new Vector2Int(x, y);
            }

        return new Vector2Int(-1, -1);
    }

    public void Clear()
    {
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
                pieces[x, y] = null;
    }
}
