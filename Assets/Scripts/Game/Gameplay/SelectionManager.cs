using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    public Piece selectedPiece;
    public Tile selectedTile;

    private IChessRules rules;
    private MoveGenerator moveGenerator;

    private HashSet<Vector2Int> validMoves = new HashSet<Vector2Int>();

    protected override void Awake()
    {
        base.Awake();
        rules = new ChessRules();
        moveGenerator = new MoveGenerator(rules);
    }

    // =====================================================
    // PIECE SELECTION
    // =====================================================

    public void OnPieceSelected(Piece piece)
    {
        if (piece == null)
            return;

        // Turn authority handled by GameMode
        if (!GameContext.Instance.GameMode.IsMyTurn(piece.teamColor))
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

    // =====================================================
    // TILE SELECTION
    // =====================================================

    public void OnTileSelected(Tile tile)
    {
        if (selectedPiece == null || tile == null)
            return;

        if (!GameContext.Instance.GameMode
            .IsMyTurn(selectedPiece.teamColor))
            return;

        if (!validMoves.Contains(tile.boardPosition))
            return;

        MoveExecutor.Instance.ExecuteMove(
            selectedPiece,
            tile.boardPosition
        );

        ClearSelection();
    }

    // =====================================================
    // MOVE GENERATION (READ-ONLY)
    // =====================================================

    void ShowPossibleMoves(Piece piece)
    {
        validMoves = moveGenerator.GetLegalMoves(
            piece,
            BoardManager.Instance.BoardState
        );

        foreach (Vector2Int pos in validMoves)
        {
            Tile tile = BoardManager.Instance.GetTileAt(pos.x, pos.y);
            if (tile == null) continue;

            if (BoardManager.Instance.GetPieceAt(pos.x, pos.y) == null)
            {
                TileHighlight.Instance.HighlightMoveTile(tile);
            }
            else
            {
                TileHighlight.Instance.HighlightCaptureTile(tile);
            }
        }
    }

    // =====================================================
    // UTILITIES
    // =====================================================

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

    // =====================================================
    // USED BY TurnManager (TEMP – Phase 4 will move this)
    // =====================================================

    public bool HasAnyLegalMove(TeamColor team)
    {
        BoardState board = BoardManager.Instance.BoardState;

        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Piece piece = board.GetPieceAt(x, y);
                if (piece == null || piece.teamColor != team)
                    continue;

                var moves = moveGenerator.GetLegalMoves(piece, board);
                if (moves.Count > 0)
                    return true;
            }
        }

        return false;
    }
}
