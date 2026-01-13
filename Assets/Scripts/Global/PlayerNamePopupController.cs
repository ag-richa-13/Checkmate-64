using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class PlayerNamePopupController : MonoBehaviour
{
    public TMP_InputField input;
    public Button continueButton;

    void Awake()
    {
        continueButton.onClick.AddListener(SaveName);
    }

    void SaveName()
    {
        string name = input.text.Trim();
        if (string.IsNullOrEmpty(name))
            return;

        PlayerData.Name = name;
        PhotonNetwork.NickName = name;

        gameObject.SetActive(false);
    }
}
