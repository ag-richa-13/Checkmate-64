using UnityEngine;

public class MoveData
{
    public Piece piece;
    public Vector2Int from;
    public Vector2Int to;

    // Special flags
    public bool isCapture;
    public bool isEnPassant;
    public bool isCastling;
    public bool isPromotion;

    // Extra data
    public Vector2Int capturedPiecePos;
    public Vector2Int rookFrom;
    public Vector2Int rookTo;
}
