using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button playOffline;
    public Button playFriends;
    public Button playComputer;

    void Start()
    {
        playOffline.onClick.AddListener(() =>
            SceneLoader.Instance.LoadScene(SceneLoader.SceneType.OfflineGame));

        playFriends.onClick.AddListener(() =>
            Debug.Log("Multiplayer coming soon"));

        playComputer.onClick.AddListener(() =>
            Debug.Log("AI coming soon"));
    }
}
