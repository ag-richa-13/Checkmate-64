using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    public GameObject tilePrefab;
    public Sprite whiteTile;
    public Sprite blackTile;

    public Transform[] rows;
    private Tile[,] board = new Tile[8, 8];

    private Piece[,] pieces = new Piece[8, 8];

    void Start()
    {
        GenerateBoard();
        PieceSpawner.Instance.SpawnAllPieces(); // CALL AFTER BOARD READY
    }

    void GenerateBoard()
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                GameObject tileObj = Instantiate(tilePrefab, rows[row]);
                Tile tile = tileObj.GetComponent<Tile>();

                bool isWhite = (row + col) % 2 == 0;
                tile.Init(col, 7 - row, isWhite ? whiteTile : blackTile);

                board[col, 7 - row] = tile;
            }
        }
    }
    public Vector2Int GetKingPosition(TeamColor team)
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Piece p = pieces[x, y];
                if (p != null && p.pieceType == PieceType.King && p.teamColor == team)
                    return new Vector2Int(x, y);
            }
        }
        return new Vector2Int(-1, -1);
    }

    public Tile GetTileAt(int x, int y)
    {
        return board[x, y];
    }

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

    public void ClearBoard()
    {
        // Clear pieces array
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                pieces[x, y] = null;
            }
        }

        // Destroy piece GameObjects
        foreach (Piece p in FindObjectsByType<Piece>(FindObjectsSortMode.None))
        {
            Destroy(p.gameObject);
        }
    }

}
