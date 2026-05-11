using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CommonPopupController : Singleton<CommonPopupController>
{
    [Header("Popup UI")]
    public GameObject popupRoot;
    public TMP_Text titleText;
    public TMP_Text messageText;
    public Button leftButton;
    public Button rightButton;
    public TMP_Text leftButtonText;
    public TMP_Text rightButtonText;

    void Start()
    {
        popupRoot.SetActive(false);
    }

    // ================= DRAW =================
    public void ShowDrawConfirm()
    {
        ShowPopup(
            "Draw Match?",
            "Do you want to declare a draw?",
            "No",
            "Yes",
            ClosePopup,
            () =>
            {
                MatchController.Instance.DeclareDraw();
                ClosePopup();
                ShowResult("Draw");
            }
        );
    }

    // ================= RESIGN =================
    public void ShowResignConfirm()
    {
        ShowPopup(
            "Resign Game?",
            "Are you sure you want to resign?",
            "No",
            "Yes",
            ClosePopup,
            () =>
            {
                TeamColor myColor =
                    PhotonPlayerData.LocalSide == PlayerSide.White
                        ? TeamColor.White
                        : TeamColor.Black;

                ClosePopup();

                ShowResult(
                    myColor == TeamColor.White
                        ? "Black Wins (Resign)"
                        : "White Wins (Resign)"
                );
            }
        );
    }


    // ================= EXIT =================
    public void ShowExitConfirm()
    {
        ShowPopup(
            "Exit Game?",
            "Your progress will be lost.",
            "No",
            "Yes",
            ClosePopup,
            () =>
            {
                SceneLoader.Instance.LoadScene(SceneLoader.SceneType.MainMenu);
            }
        );
    }

    // ================= RESULT =================
    public void ShowResult(string resultTextValue)
    {
        ShowPopup(
            "Game Over",
            resultTextValue,
            "Exit",
            "Restart",
            () =>
            {
                SceneManager.LoadScene("MainMenuScene");
            },
            () =>
            {
                GameManager.Instance.RestartGame();
                ClosePopup();
            }
        );
    }

    // ================= CORE =================
    void ShowPopup(
        string title,
        string message,
        string leftTxt,
        string rightTxt,
        UnityEngine.Events.UnityAction leftAction,
        UnityEngine.Events.UnityAction rightAction
    )
    {
        popupRoot.SetActive(true);

        titleText.text = title;
        messageText.text = message;

        leftButtonText.text = leftTxt;
        rightButtonText.text = rightTxt;

        leftButton.onClick.RemoveAllListeners();
        rightButton.onClick.RemoveAllListeners();

        leftButton.onClick.AddListener(leftAction);
        rightButton.onClick.AddListener(rightAction);
    }

    public void ClosePopup()
    {
        popupRoot.SetActive(false);
    }
}
