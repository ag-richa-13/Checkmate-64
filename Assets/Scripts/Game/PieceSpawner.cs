using UnityEngine;

public class PieceSpawner : Singleton<PieceSpawner>
{
    [Header("References")]

    public GameObject piecePrefab;

    [Header("White Sprites")]
    public Sprite whitePawn;
    public Sprite whiteRook;
    public Sprite whiteKnight;
    public Sprite whiteBishop;
    public Sprite whiteQueen;
    public Sprite whiteKing;

    [Header("Black Sprites")]
    public Sprite blackPawn;
    public Sprite blackRook;
    public Sprite blackKnight;
    public Sprite blackBishop;
    public Sprite blackQueen;
    public Sprite blackKing;

    // void Start()
    // {
    //     SpawnAllPieces();
    // }

    public void SpawnAllPieces()
    {
        // White pieces
        SpawnTeam(TeamColor.White);

        // Black pieces
        SpawnTeam(TeamColor.Black);
    }

    void SpawnTeam(TeamColor team)
    {
        int pawnRow = team == TeamColor.White ? 1 : 6;
        int backRow = team == TeamColor.White ? 0 : 7;

        // Pawns
        for (int x = 0; x < 8; x++)
        {
            SpawnPiece(PieceType.Pawn, team, x, pawnRow);
        }

        // Rooks
        SpawnPiece(PieceType.Rook, team, 0, backRow);
        SpawnPiece(PieceType.Rook, team, 7, backRow);

        // Knights
        SpawnPiece(PieceType.Knight, team, 1, backRow);
        SpawnPiece(PieceType.Knight, team, 6, backRow);

        // Bishops
        SpawnPiece(PieceType.Bishop, team, 2, backRow);
        SpawnPiece(PieceType.Bishop, team, 5, backRow);

        // Queen
        SpawnPiece(PieceType.Queen, team, 3, backRow);

        // King
        SpawnPiece(PieceType.King, team, 4, backRow);
    }

    void SpawnPiece(PieceType type, TeamColor team, int x, int y)
    {
        Tile tile = BoardManager.Instance.GetTileAt(x, y);
        if (tile == null) return;

        GameObject pieceObj = Instantiate(piecePrefab, tile.transform);
        Piece piece = pieceObj.GetComponent<Piece>();

        // 🔥 FORCE CENTER POSITION (UI FIX)
        RectTransform rt = pieceObj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.localScale = Vector3.one;

        Sprite sprite = GetSprite(type, team);
        piece.Init(type, team, new Vector2Int(x, y), sprite);

        Debug.Log($"Spawning {type} {team} at {x},{y}");
    }


    Sprite GetSprite(PieceType type, TeamColor team)
    {
        if (team == TeamColor.White)
        {
            return type switch
            {
                PieceType.Pawn => whitePawn,
                PieceType.Rook => whiteRook,
                PieceType.Knight => whiteKnight,
                PieceType.Bishop => whiteBishop,
                PieceType.Queen => whiteQueen,
                PieceType.King => whiteKing,
                _ => null
            };
        }
        else
        {
            return type switch
            {
                PieceType.Pawn => blackPawn,
                PieceType.Rook => blackRook,
                PieceType.Knight => blackKnight,
                PieceType.Bishop => blackBishop,
                PieceType.Queen => blackQueen,
                PieceType.King => blackKing,
                _ => null
            };
        }
    }
}
