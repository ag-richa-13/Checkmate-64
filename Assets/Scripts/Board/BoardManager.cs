using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    public GameObject tilePrefab;
    public Sprite whiteTile;
    public Sprite blackTile;

    public Transform[] rows;
    private Tile[,] board = new Tile[8, 8];


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

    public Tile GetTileAt(int x, int y)
    {
        return board[x, y];
    }
}
