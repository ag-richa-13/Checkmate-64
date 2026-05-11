using UnityEngine;

public class GameSceneBootstrap : MonoBehaviour
{
    [SerializeField] private bool isOnlineGame;

    void Awake()
    {
        if (isOnlineGame)
        {
            GameContext.Instance.SetGameMode(
                new OnlineGameMode()
            );
        }
        else
        {
            GameContext.Instance.SetGameMode(
                new OfflineGameMode()
            );
        }
        GameContext.Instance.Init();
    }

}
