using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FooterController : MonoBehaviour
{
    [Header("Buttons")]
    public Button homeButton;
    public Button friendsButton;
    public Button profileButton;

    [Header("Panels")]
    public GameObject homePanel;
    public GameObject lobbyPanel;
    public GameObject profilePanel;

    [Header("Texts")]
    public TMP_Text homeText;
    public TMP_Text friendsText;
    public TMP_Text profileText;

    [Header("Button Images")]
    public Image homeImage;
    public Image friendsImage;
    public Image profileImage;

    Color inactiveTextColor;
    Color activeTextColor;

    void Awake()
    {
        inactiveTextColor = Hex("#9CA3AF");
        activeTextColor = Hex("#F8FAFC");

        // Button listeners
        homeButton.onClick.AddListener(() => ActivateTab(Tab.Home));
        friendsButton.onClick.AddListener(() => ActivateTab(Tab.Friends));
        profileButton.onClick.AddListener(() => ActivateTab(Tab.Profile));
    }

    void Start()
    {
        ActivateTab(Tab.Home); // default
    }

    enum Tab
    {
        Home,
        Friends,
        Profile
    }

    void ActivateTab(Tab tab)
    {
        // Panels
        homePanel.SetActive(tab == Tab.Home);
        lobbyPanel.SetActive(tab == Tab.Friends);
        profilePanel.SetActive(tab == Tab.Profile);

        // Visual states
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
