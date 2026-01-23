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

        GlobalLoaderController.Instance.Show();

        PlayerData.Name = name;
        PhotonNetwork.NickName = name;

        gameObject.SetActive(false);

        // Give UI one frame to refresh welcome text
        Invoke(nameof(HideLoader), 0.2f);
    }

    void HideLoader()
    {
        GlobalLoaderController.Instance.Hide();
    }
}
