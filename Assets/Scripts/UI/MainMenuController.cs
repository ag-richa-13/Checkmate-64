using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button playOffline;
    public Button playFriends;
    public Button playComputer;

    void Start()
    {
        playOffline.onClick.AddListener(() =>
            SceneManager.LoadScene("OfflineScene"));

        playFriends.onClick.AddListener(() =>
            Debug.Log("Multiplayer coming soon"));

        playComputer.onClick.AddListener(() =>
            Debug.Log("AI coming soon"));
    }
}
