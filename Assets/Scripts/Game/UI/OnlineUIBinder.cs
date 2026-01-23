using UnityEngine;
using TMPro;

public class OnlineUIBinder : MonoBehaviour
{
    [Header("Header UI")]
    [SerializeField] private TMP_Text whitePlayerNameText;
    [SerializeField] private TMP_Text blackPlayerNameText;

    void Start()
    {
        BindPlayerNames();
    }

    void BindPlayerNames()
    {
        whitePlayerNameText.text = PhotonPlayerData.WhitePlayerName;
        blackPlayerNameText.text = PhotonPlayerData.BlackPlayerName;
    }
}
