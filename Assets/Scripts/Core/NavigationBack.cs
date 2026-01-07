using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // ✅ IMPORTANT

public class NavigationBack : Singleton<NavigationBack>
{
    [Header("Scene Names")]
    public string mainMenuScene = "MainMenuScene";
    public string gameScene = "OfflineScene";

    void Update()
    {
        // Android back OR Escape key
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            HandleBack();
        }

        // Android hardware back (extra safety)
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                HandleBack();
            }
        }
    }

    public void HandleBack()
    {
        string current = SceneManager.GetActiveScene().name;

        // ❌ GameScene me back disabled
        if (current == gameScene)
        {
            return;
        }

        if (current == mainMenuScene)
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}
