using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private GameObject profilePanel;

    [Header("Top Buttons")]
    [SerializeField] private Button playOffline;
    [SerializeField] private Button playFriends;
    [SerializeField] private Button playComputer;

    void Awake()
    {
        // Wire Main Menu buttons
        playOffline.onClick.AddListener(PlayOffline);
        playFriends.onClick.AddListener(OpenFriends);
        playComputer.onClick.AddListener(PlayComputer);
    }

    void Start()
    {
        GlobalLoaderController.Instance.Show();
        GlobalPopupManager.Instance.OnMainMenuLoaded();
        Invoke(nameof(InitUI), 0.2f);
    }

    void InitUI()
    {
        GlobalLoaderController.Instance.Hide();
        ShowHome();
    }
    void OnEnable()
    {
        NavigationBack.OnBackRequested += HandleBackEvent;
    }

    void OnDisable()
    {
        NavigationBack.OnBackRequested -= HandleBackEvent;
    }

    private bool HandleBackEvent()
    {
        // If not on Home → go back to Home
        if (!homePanel.activeSelf)
        {
            ShowHome();
            return true; // consumed
        }

        // On Home → allow app exit
        return false;
    }
    /* ---------------- PUBLIC PANEL API ---------------- */

    public void ShowHome()
    {
        ActivateOnly(homePanel);
    }

    public void ShowFriends()
    {
        if (!GlobalPopupManager.Instance.CanUseOnlineFeature())
            return;

        GlobalLoaderController.Instance.Show();
        Invoke(nameof(OpenLobby), 0.2f);
    }

    public void ShowProfile()
    {
        if (!GlobalPopupManager.Instance.CanUseOnlineFeature())
            return;

        GlobalLoaderController.Instance.Show();
        Invoke(nameof(OpenProfile), 0.2f);
    }

    /* ---------------- PRIVATE HELPERS ---------------- */

    private void OpenLobby()
    {
        ActivateOnly(lobbyPanel);
        GlobalLoaderController.Instance.Hide();
    }

    private void OpenProfile()
    {
        ActivateOnly(profilePanel);
        GlobalLoaderController.Instance.Hide();
    }

    private void ActivateOnly(GameObject panel)
    {
        homePanel.SetActive(false);
        lobbyPanel.SetActive(false);
        profilePanel.SetActive(false);

        panel.SetActive(true);
    }

    private void PlayOffline()
    {
        SceneLoader.Instance.LoadScene(SceneLoader.SceneType.OfflineGame);
    }

    private void PlayComputer()
    {
        Debug.Log("AI coming soon");
    }

    private void OpenFriends()
    {
        ShowFriends();
    }

    /* ---------------- BACK HANDLING ---------------- */

    public bool HandleBack()
    {
        if (!homePanel.activeSelf)
        {
            ShowHome();
            return true;
        }

        return false;
    }
}
