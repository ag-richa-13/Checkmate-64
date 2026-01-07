using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum PieceType
{
    Pawn,
    Rook,
    Knight,
    Bishop,
    Queen,
    King
}

public enum TeamColor
{
    White,
    Black
}

public class Piece : MonoBehaviour, IPointerClickHandler
{
    public PieceType pieceType;
    public TeamColor teamColor;
    public Vector2Int boardPosition;
    public Image image;
    public bool justMovedTwoSteps;
    public bool hasMoved;


    public void Init(PieceType type, TeamColor color, Vector2Int pos, Sprite sprite)
    {
        pieceType = type;
        teamColor = color;
        boardPosition = pos;
        image.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectionManager sm = SelectionManager.Instance;

        // CASE 1: No piece selected → select this
        if (sm.selectedPiece == null)
        {
            sm.OnPieceSelected(this);
            return;
        }

        // CASE 2: Friendly piece clicked → re-select
        if (sm.selectedPiece.teamColor == teamColor)
        {
            sm.OnPieceSelected(this);
            return;
        }

        // CASE 3: Enemy piece clicked → capture attempt
        Tile tile = BoardManager.Instance.GetTileAt(
            boardPosition.x,
            boardPosition.y
        );

        sm.OnTileSelected(tile);
    }


    public bool IsEnemy(Piece other)
    {
        return other != null && other.teamColor != teamColor;
    }

}