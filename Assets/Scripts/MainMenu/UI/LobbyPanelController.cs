using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LobbyPanelController : MonoBehaviour
{
    [Header("Top Bar")]
    [SerializeField] private TMP_Text playerNameText;

    [Header("Room UI")]
    [SerializeField] private TMP_Text roomCodeText;
    [SerializeField] private Button copyRoomCodeButton;
    [SerializeField] private TMP_InputField roomCodeInputField;

    [Header("Player Slots")]
    [SerializeField] private Transform playerListParent;
    [SerializeField] private GameObject playerSlotPrefab;

    [Header("Action Buttons")]
    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private Button startGameButton;

    PlayerSlotUI slotA;
    PlayerSlotUI slotB;

    readonly Color green = new Color(0.2f, 0.8f, 0.4f);
    readonly Color yellow = new Color(0.95f, 0.75f, 0.2f);
    readonly Color gray = new Color(0.6f, 0.6f, 0.6f);

    void Awake()
    {
        createRoomButton.onClick.AddListener(OnCreateRoom);
        joinRoomButton.onClick.AddListener(OnJoinRoom);
        copyRoomCodeButton.onClick.AddListener(OnCopyCode);
        startGameButton.onClick.AddListener(OnStartGame);

        roomCodeInputField.onValueChanged.AddListener(OnRoomCodeChanged);
    }

    void OnEnable()
    {
        SetupUI();
        RegisterEvents();
        NavigationBack.OnBackRequested += OnBack;
    }

    void OnDisable()
    {
        UnregisterEvents();
        NavigationBack.OnBackRequested -= OnBack;
    }


    bool OnBack()
    {
        // Leave room if inside one
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }

        // Go back to Home
        FindFirstObjectByType<MainMenuController>().ShowHome();
        return true;
    }

    void SetupUI()
    {
        ClearSlots();
        PhotonPlayerData.Reset();

        playerNameText.text = PlayerData.Name;

        slotA = Instantiate(playerSlotPrefab, playerListParent).GetComponent<PlayerSlotUI>();
        slotB = Instantiate(playerSlotPrefab, playerListParent).GetComponent<PlayerSlotUI>();

        slotA.SetPlayer(PlayerData.Name, "Ready", green);
        slotB.SetEmpty("Create Room", gray);

        roomCodeText.text = "";
        copyRoomCodeButton.interactable = false;

        createRoomButton.gameObject.SetActive(true);
        createRoomButton.interactable = true;

        joinRoomButton.gameObject.SetActive(false);
        joinRoomButton.interactable = false;

        startGameButton.gameObject.SetActive(false);
        startGameButton.interactable = false;
    }

    void RegisterEvents()
    {
        PhotonRoomManager.Instance.RoomCreated += OnRoomCreated;
        PhotonRoomManager.Instance.RoomJoined += OnRoomJoined;
        PhotonRoomManager.Instance.PlayerJoined += RefreshPlayerSlots;
    }

    void UnregisterEvents()
    {
        PhotonRoomManager.Instance.RoomCreated -= OnRoomCreated;
        PhotonRoomManager.Instance.RoomJoined -= OnRoomJoined;
        PhotonRoomManager.Instance.PlayerJoined -= RefreshPlayerSlots;
    }

    void OnCreateRoom()
    {
        GlobalLoaderController.Instance.Show();
        PhotonRoomManager.Instance.CreateRoom();
        createRoomButton.interactable = false;
        joinRoomButton.gameObject.SetActive(false);

        startGameButton.gameObject.SetActive(true);
        startGameButton.interactable = false;

        slotB.SetEmpty("Waiting for player...", yellow);
    }

    void OnRoomCreated(string code)
    {
        roomCodeText.text = code;
        copyRoomCodeButton.interactable = true;
        GlobalLoaderController.Instance.Hide();
    }

    void OnRoomCodeChanged(string value)
    {
        if (PhotonPlayerData.IsHost)
            return;

        joinRoomButton.gameObject.SetActive(!string.IsNullOrEmpty(value));
        joinRoomButton.interactable = !string.IsNullOrEmpty(value);
    }

    void OnJoinRoom()
    {
        GlobalLoaderController.Instance.Show();
        PhotonRoomManager.Instance.JoinRoom(roomCodeInputField.text.Trim());

        joinRoomButton.gameObject.SetActive(false);
        createRoomButton.interactable = false;

        startGameButton.gameObject.SetActive(true);
        startGameButton.interactable = true;
    }

    void OnRoomJoined(string code)
    {
        roomCodeText.text = code;
        copyRoomCodeButton.interactable = !string.IsNullOrEmpty(code);
        GlobalLoaderController.Instance.Hide();
    }

    void RefreshPlayerSlots(string _)
    {
        if (!PhotonNetwork.InRoom)
            return;

        Player[] players = PhotonNetwork.PlayerList;
        if (players.Length < 2)
            return;

        Player host = PhotonNetwork.MasterClient;
        Player joiner = players[0] == host ? players[1] : players[0];

        bool isHostView = PhotonNetwork.IsMasterClient;

        if (isHostView)
        {
            slotA.SetPlayer(host.NickName, "Start Game", green);
            slotB.SetPlayer(joiner.NickName, "2nd Player", gray);
        }
        else
        {
            slotA.SetPlayer(host.NickName, "Host", gray);
            slotB.SetPlayer(joiner.NickName, "Start Game", green);
        }

        startGameButton.interactable = PhotonNetwork.IsMasterClient;
    }

    void OnStartGame()
    {
        GlobalLoaderController.Instance.Show();
        PhotonRoomManager.Instance.StartGame();
    }

    void OnCopyCode()
    {
        if (string.IsNullOrEmpty(roomCodeText.text))
            return;

        GUIUtility.systemCopyBuffer = roomCodeText.text;
    }

    void ClearSlots()
    {
        foreach (Transform child in playerListParent)
            Destroy(child.gameObject);
    }


}
