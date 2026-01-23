using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using System.Collections;

public class ProfileUIController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button saveButton;
    [SerializeField] private TMP_Text promptText;

    [Header("Prompt Settings")]
    [SerializeField] private Color successColor = new Color(0.2f, 0.8f, 0.4f); // Green

    void Awake()
    {
        saveButton.onClick.AddListener(OnSaveClicked);
    }

    void OnEnable()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        // Clear prompt on panel open
        promptText.gameObject.SetActive(false);

        // Fill current name if exists
        if (PlayerData.HasName())
            nameInputField.text = PlayerData.Name;
        else
            nameInputField.text = string.Empty;
    }

    void OnSaveClicked()
    {
        if (!InternetChecker.IsConnected())
        {
            GlobalPopupManager.Instance.CanUseOnlineFeature();
            return;
        }

        string newName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(newName))
            return;

        if (PlayerData.Name == newName)
            return;

        GlobalLoaderController.Instance.Show();

        PlayerData.Name = newName;
        PhotonNetwork.NickName = newName;

        StartCoroutine(DelayedSuccess());
    }
    private IEnumerator DelayedSuccess()
    {
        yield return new WaitForSeconds(0.2f);
        GlobalLoaderController.Instance.Hide();
        ShowSuccessPrompt("Name updated successfully");
    }

    void ShowSuccessPrompt(string message)
    {
        promptText.text = message;
        promptText.color = successColor;
        promptText.gameObject.SetActive(true);
    }
}