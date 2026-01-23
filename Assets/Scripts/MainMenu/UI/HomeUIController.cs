using UnityEngine;
using TMPro;

public class HomeUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text welcomeText;

    void OnEnable()
    {
        RefreshWelcomeText();
        PlayerData.OnNameUpdated += RefreshWelcomeText;
    }

    void OnDisable()
    {
        PlayerData.OnNameUpdated -= RefreshWelcomeText;
    }

    void RefreshWelcomeText()
    {
        if (!InternetChecker.IsConnected())
        {
            welcomeText.text = "Welcome to Offline Chess";
            return;
        }

        if (!PlayerData.HasName())
        {
            welcomeText.text = "Welcome Player";
            return;
        }

        welcomeText.text = $"Welcome, {PlayerData.Name}";
    }

    private void OnBackButtonPressed()
    {
        Application.Quit();
    }
}
