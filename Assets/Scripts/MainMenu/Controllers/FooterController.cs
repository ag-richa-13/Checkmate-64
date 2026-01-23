using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FooterController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button homeButton;
    [SerializeField] private Button friendsButton;
    [SerializeField] private Button profileButton;

    [Header("Texts")]
    [SerializeField] private TMP_Text homeText;
    [SerializeField] private TMP_Text friendsText;
    [SerializeField] private TMP_Text profileText;

    [Header("Images")]
    [SerializeField] private Image homeImage;
    [SerializeField] private Image friendsImage;
    [SerializeField] private Image profileImage;

    [SerializeField] private MainMenuController mainMenu;

    private Color inactiveTextColor;
    private Color activeTextColor;

    private enum Tab
    {
        Home,
        Friends,
        Profile
    }

    void Awake()
    {
        inactiveTextColor = Hex("#9CA3AF");
        activeTextColor = Hex("#F8FAFC");

        homeButton.onClick.AddListener(OnHome);
        friendsButton.onClick.AddListener(OnFriends);
        profileButton.onClick.AddListener(OnProfile);
    }

    void Start()
    {
        SetActiveTab(Tab.Home);
    }

    /* ---------------- BUTTON HANDLERS ---------------- */

    void OnHome()
    {
        mainMenu.ShowHome();
        SetActiveTab(Tab.Home);
    }

    void OnFriends()
    {
        if (!GlobalPopupManager.Instance.CanUseOnlineFeature())
            return;

        mainMenu.ShowFriends();
        SetActiveTab(Tab.Friends);
    }

    void OnProfile()
    {
        if (!GlobalPopupManager.Instance.CanUseOnlineFeature())
            return;

        mainMenu.ShowProfile();
        SetActiveTab(Tab.Profile);
    }

    /* ---------------- UI STATE ---------------- */

    void SetActiveTab(Tab tab)
    {
        SetState(homeImage, homeText, tab == Tab.Home);
        SetState(friendsImage, friendsText, tab == Tab.Friends);
        SetState(profileImage, profileText, tab == Tab.Profile);
    }

    void SetState(Image img, TMP_Text txt, bool active)
    {
        Color c = img.color;
        c.a = active ? 1f : 0f;
        img.color = c;

        txt.color = active ? activeTextColor : inactiveTextColor;
    }

    Color Hex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }
}
