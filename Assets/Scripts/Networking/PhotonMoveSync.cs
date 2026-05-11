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

    public void SendMove(Vector2Int from, Vector2Int to)
    {
        if (!OnlineTurnManager.Instance.IsMyTurn())
            return;

        // 1️⃣ Send move to opponent
        photonView.RPC(
            nameof(RPC_ReceiveMove),
            RpcTarget.Others,
            from.x, from.y,
            to.x, to.y
        );

        // 2️⃣ Ask MASTER to switch turn
        photonView.RPC(
            nameof(RPC_RequestTurnSwitch),
            RpcTarget.MasterClient
        );
    }

    [PunRPC]
    void RPC_RequestTurnSwitch()
    {
        OnlineTurnManager.Instance.RequestTurnSwitch();
    }

    [PunRPC]
    void RPC_ReceiveMove(int fromX, int fromY, int toX, int toY)
    {
        ApplyRemoteMove(
            new Vector2Int(fromX, fromY),
            new Vector2Int(toX, toY)
        );
    }

    void ApplyRemoteMove(Vector2Int from, Vector2Int to)
    {
        Piece piece = BoardManager.Instance.GetPieceAt(from.x, from.y);
        if (piece == null) return;

        MoveExecutor.Instance.ExecuteMove(piece, to, true);
    }
}
