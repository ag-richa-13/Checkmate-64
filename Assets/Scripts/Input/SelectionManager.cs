using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : Singleton<SelectionManager>
{
    public Piece selectedPiece;
    public Tile selectedTile;

    private List<Tile> highlightedTiles = new List<Tile>();

    public void OnPieceSelected(Piece piece)
    {
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

        // Move logic NEXT STEP
        Debug.Log($"Tile clicked: {tile.boardPosition}");
    }

    void ShowPossibleMoves(Piece piece)
    {
        // Temporary demo logic (Pawn-like movement)
        Vector2Int dir = piece.teamColor == TeamColor.White ? Vector2Int.up : Vector2Int.down;

        Vector2Int targetPos = piece.boardPosition + dir;

        Tile moveTile = BoardManager.Instance.GetTileAt(targetPos.x, targetPos.y);
        if (moveTile != null)
        {
            TileHighlight.Instance.HighlightMoveTile(moveTile);
            highlightedTiles.Add(moveTile);
        }
    }

    void ClearSelection()
    {
        TileHighlight.Instance.ClearAll();
        selectedPiece = null;
        selectedTile = null;
        highlightedTiles.Clear();
    }
}
