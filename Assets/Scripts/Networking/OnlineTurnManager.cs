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
        if (PhotonNetwork.IsMasterClient)
        {
            CurrentTurn = TeamColor.White;
            photonView.RPC(nameof(RPC_SetTurn), RpcTarget.All, (int)CurrentTurn);
        }
    }

    public bool IsMyTurn()
    {
        TeamColor myColor =
            PhotonPlayerData.LocalSide == PlayerSide.White
                ? TeamColor.White
                : TeamColor.Black;

        return CurrentTurn == myColor;
    }

    [PunRPC]
    void RPC_SetTurn(int turn)
    {
        CurrentTurn = (TeamColor)turn;

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateTurnUI(CurrentTurn);
    }

    [PunRPC]
    void RPC_SwitchTurn()
    {
        CurrentTurn =
            CurrentTurn == TeamColor.White
                ? TeamColor.Black
                : TeamColor.White;

        if (UIManager.Instance != null)
            UIManager.Instance.UpdateTurnUI(CurrentTurn);
    }

    public void RequestTurnSwitch()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        photonView.RPC(nameof(RPC_SwitchTurn), RpcTarget.All);
    }

}
