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

    public void Init(PieceType type, TeamColor color, Vector2Int pos, Sprite sprite)
    {
        pieceType = type;
        teamColor = color;
        boardPosition = pos;
        image.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectionManager.Instance.OnPieceSelected(this);
    }
}