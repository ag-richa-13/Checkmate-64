using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System;

public class PhotonRoomManager : MonoBehaviourPunCallbacks
{
    public static PhotonRoomManager Instance;

    public event Action<string> RoomCreated;
    public event Action<string> RoomJoined;
    public event Action<string> PlayerJoined;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ================= CREATE =================

    public void CreateRoom()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogError("Photon not ready");
            return;
        }

        PhotonPlayerData.SetHost();

        string roomCode = GenerateRoomCode();

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = 2,
            IsVisible = false,
            IsOpen = true
        };

        PhotonNetwork.CreateRoom(roomCode, options);
    }

    // ================= JOIN =================

    public void JoinRoom(string code)
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            Debug.LogError("Photon not ready");
            return;
        }

        PhotonPlayerData.SetJoiner();
        PhotonNetwork.JoinRoom(code);
    }

    // ================= START =================

    public void StartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        PhotonNetwork.LoadLevel("OnlineGameScene");
    }

    // ================= CALLBACKS =================

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created");
        GlobalLoaderController.Instance.Hide();
        RoomCreated?.Invoke(PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        RoomJoined?.Invoke(PhotonNetwork.CurrentRoom.Name);
        PlayerJoined?.Invoke(null);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerJoined?.Invoke(newPlayer.NickName);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError(message);
        GlobalLoaderController.Instance.Hide();
    }

    // ================= UTILS =================

    string GenerateRoomCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string code = "";

        for (int i = 0; i < 6; i++)
            code += chars[UnityEngine.Random.Range(0, chars.Length)];

        return code;
    }
}
