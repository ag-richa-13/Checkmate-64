using Photon.Pun;
using UnityEngine;

public class OnlineTurnManager : MonoBehaviourPun
{
    public static OnlineTurnManager Instance;

    public TeamColor CurrentTurn { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        InitializeTurn();
    }

    // ================= INIT =================

    void InitializeTurn()
    {
        // White always starts
        CurrentTurn = TeamColor.White;

        UpdateTurnUI();
    }

    // ================= AUTHORITY =================

    public bool IsMyTurn()
    {
        TeamColor myColor =
            PhotonPlayerData.LocalSide == PlayerSide.White
                ? TeamColor.White
                : TeamColor.Black;

        return CurrentTurn == myColor;
    }

    // ================= TURN SWITCH =================

    public void EndTurn()
    {
        // Only the player who made the move can request turn end
        if (!IsMyTurn())
            return;

        photonView.RPC(nameof(RPC_SwitchTurn), RpcTarget.All);
    }

    [PunRPC]
    void RPC_SwitchTurn()
    {
        CurrentTurn =
            CurrentTurn == TeamColor.White
                ? TeamColor.Black
                : TeamColor.White;

        UpdateTurnUI();
    }

    // ================= UI =================

    void UpdateTurnUI()
    {
        // We don't enable UIManager fully yet,
        // but this keeps things future-safe
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTurnUI(CurrentTurn);
        }
    }
}
