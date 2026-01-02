using UnityEngine;
using UnityEngine.SceneManagement;

public class BackNavigationManager : Singleton<BackNavigationManager>
{
    [Header("Scene Names")]
    public string mainMenuScene = "MainMenuScene";
    public string gameScene = "OfflineScene";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleBack();
        }
    }

    public void HandleBack()
    {
        string current = SceneManager.GetActiveScene().name;

        if (current == gameScene)
        {
            SceneManager.LoadScene(mainMenuScene);
        }
        else
        {
            Application.Quit();
        }
    }
}
