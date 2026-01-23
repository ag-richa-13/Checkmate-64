using Photon.Pun;
using UnityEngine;

public class OnlineGameManager : MonoBehaviour
{
    void Awake()
    {
        // Decide white / black as soon as scene loads
        PhotonPlayerData.DecideSide();

    }

    void Start()
    {
        Debug.Log($"Local Side: {PhotonPlayerData.LocalSide}");
        Debug.Log($"White: {PhotonPlayerData.WhitePlayerName}");
        Debug.Log($"Black: {PhotonPlayerData.BlackPlayerName}");
    }
}
