using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public enum SceneType
    {
        Splash,
        MainMenu,
        OfflineGame,
        OnlineGame
    }


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(SceneType scene)
    {
        SceneManager.LoadScene(GetSceneName(scene));
    }

    public void LoadSceneDelayed(SceneType scene, float delay)
    {
        Invoke(nameof(LoadDelayed), delay);
        _delayedScene = scene;
    }

    // ================= PRIVATE =================

    private SceneType _delayedScene;

    void LoadDelayed()
    {
        LoadScene(_delayedScene);
    }

    string GetSceneName(SceneType scene)
    {
        switch (scene)
        {
            case SceneType.Splash:
                return "SplashScene";

            case SceneType.MainMenu:
                return "MainMenuScene";

            case SceneType.OfflineGame:
                return "OfflineScene";

            case SceneType.OnlineGame:
                return "OnlineGameScene";

            default:
                Debug.LogError("Scene not mapped!");
                return "";
        }
    }
}
