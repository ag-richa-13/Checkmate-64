using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonManager : Singleton<PhotonManager>
{
    [Header("Photon Settings")]
    public string gameVersion = "1.0";

    public bool IsConnected => PhotonNetwork.IsConnected;

    protected override void Awake()
    {
        base.Awake();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
            return;

        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }
}
