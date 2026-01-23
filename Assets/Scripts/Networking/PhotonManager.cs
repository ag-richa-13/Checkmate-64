using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonManager : Singleton<PhotonManager>, IConnectionCallbacks, ILobbyCallbacks
{
    [Header("Photon Settings")]
    [SerializeField] private string gameVersion = "1.0";

    public bool IsConnected => PhotonNetwork.IsConnected;

    protected override void Awake()
    {
        base.Awake();

        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.AddCallbackTarget(this);

        PhotonNetwork.KeepAliveInBackground = 60;
    }

    void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    // ================= CONNECT =================

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
            return;

        GlobalLoaderController.Instance.Show();

        PhotonNetwork.NickName = PlayerData.Name;

        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }

    // ================= CALLBACKS =================

    public void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");

        //  CRITICAL FIX
        PhotonNetwork.JoinLobby();
    }

    public void OnJoinedLobby()
    {
        Debug.Log("Joined Default Lobby");
        GlobalLoaderController.Instance.Hide();
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"Disconnected: {cause}");
    }

    public void OnConnected() { }
    public void OnRegionListReceived(RegionHandler regionHandler) { }
    public void OnCustomAuthenticationResponse(
        System.Collections.Generic.Dictionary<string, object> data)
    { }
    public void OnCustomAuthenticationFailed(string debugMessage) { }
    public void OnLeftLobby() { }
    public void OnRoomListUpdate(System.Collections.Generic.List<RoomInfo> roomList) { }
    public void OnLobbyStatisticsUpdate(System.Collections.Generic.List<TypedLobbyInfo> lobbyStatistics) { }
}
