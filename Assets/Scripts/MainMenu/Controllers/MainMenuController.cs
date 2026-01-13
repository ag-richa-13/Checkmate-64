using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button playOffline;
    public Button playFriends;
    public Button playComputer;

    void Start()
    {
        GlobalPopupManager.Instance.OnMainMenuLoaded();

        playOffline.onClick.AddListener(() =>
            SceneLoader.Instance.LoadScene(SceneLoader.SceneType.OfflineGame));

        playComputer.onClick.AddListener(() =>
            Debug.Log("AI coming soon"));

        playFriends.onClick.AddListener(OpenFriends);
    }

    void OpenFriends()
    {
        if (!GlobalPopupManager.Instance.CanUseOnlineFeature())
            return;

        Debug.Log("Open Lobby Panel");
    }
}
