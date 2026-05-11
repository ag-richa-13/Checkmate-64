using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    [Header("Board Setup")]
    public GameObject tilePrefab;
    public Sprite whiteTile;
    public Sprite blackTile;

    [Header("Board Layout")]
    public Transform[] rows;

    private Tile[,] board = new Tile[8, 8];

    // 🔥 SINGLE SOURCE OF TRUTH FOR PIECES
    public BoardState BoardState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        BoardState = new BoardState();
    }

    void Start()
    {
        GenerateBoard();
        PieceSpawner.Instance.SpawnAllPieces(); // AFTER board ready
    }

    // ================= BOARD GENERATION =================

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

    // ================= TILE ACCESS =================

    public Tile GetTileAt(int x, int y)
    {
        if (x < 0 || x > 7 || y < 0 || y > 7)
            return null;

        return board[x, y];
    }

    // ================= PIECE ACCESS (FORWARDERS) =================

    public Piece GetPieceAt(int x, int y)
    {
        return BoardState.GetPieceAt(x, y);
    }

    public void SetPieceAt(int x, int y, Piece piece)
    {
        BoardState.SetPieceAt(x, y, piece);
    }

    public Vector2Int GetKingPosition(TeamColor team)
    {
        return BoardState.GetKingPosition(team);
    }

    // ================= RESET =================

    public void ClearBoard()
    {
        // Clear logical board
        BoardState.Clear();

        // Destroy all piece GameObjects
        foreach (Piece p in FindObjectsByType<Piece>(FindObjectsSortMode.None))
        {
            Destroy(p.gameObject);
        }
    }
}
