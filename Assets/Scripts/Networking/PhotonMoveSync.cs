using Photon.Pun;
using UnityEngine;

public class PhotonMoveSync : MonoBehaviourPun
{
    public static PhotonMoveSync Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // =========================================================
    // CALLED BY LOCAL PLAYER AFTER A LEGAL MOVE
    // =========================================================
    public void SendMove(Vector2Int from, Vector2Int to)
    {
        // Only current-turn player can send
        if (!OnlineTurnManager.Instance.IsMyTurn())
            return;

        photonView.RPC(
            nameof(RPC_ReceiveMove),
            RpcTarget.Others,
            from.x, from.y,
            to.x, to.y
        );

        // End local turn AFTER sending
        OnlineTurnManager.Instance.EndTurn();
    }

    // =========================================================
    // RECEIVED BY REMOTE PLAYER
    // =========================================================
    [PunRPC]
    void RPC_ReceiveMove(int fromX, int fromY, int toX, int toY)
    {
        ApplyRemoteMove(
            new Vector2Int(fromX, fromY),
            new Vector2Int(toX, toY)
        );

        // End turn for remote side
        OnlineTurnManager.Instance.EndTurn();
    }

    // =========================================================
    // APPLY MOVE WITHOUT RE-SENDING
    // =========================================================
    void ApplyRemoteMove(Vector2Int from, Vector2Int to)
    {
        Piece piece = BoardManager.Instance.GetPieceAt(from.x, from.y);
        if (piece == null)
            return;

        // Direct execution path (bypasses selection clicks)
        SelectionManager.Instance.ExecuteMoveExternally(piece, to);
    }
}
