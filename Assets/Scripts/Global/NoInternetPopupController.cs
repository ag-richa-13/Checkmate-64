using UnityEngine;
using UnityEngine.UI;

public class NoInternetPopupController : MonoBehaviour
{
    public Button retryButton;
    public Button exitButton;

    void Awake()
    {
        retryButton.onClick.AddListener(TryAgain);
        exitButton.onClick.AddListener(Application.Quit);
    }

    void TryAgain()
    {
        if (InternetChecker.IsConnected())
            gameObject.SetActive(false);
    }
}
