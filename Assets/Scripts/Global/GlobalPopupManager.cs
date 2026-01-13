using UnityEngine;

public class GlobalPopupManager : Singleton<GlobalPopupManager>
{
    public GameObject namePopup;
    public GameObject noInternetPopup;

    protected override void Awake()
    {
        base.Awake();
        HideAll();
    }

    public void OnMainMenuLoaded()
    {
        if (!InternetChecker.IsConnected())
            return;

        PhotonManager.Instance.Connect();

        if (!PlayerData.HasName())
            namePopup.SetActive(true);
    }

    public bool CanUseOnlineFeature()
    {
        if (!InternetChecker.IsConnected())
        {
            noInternetPopup.SetActive(true);
            return false;
        }

        if (!PlayerData.HasName())
        {
            namePopup.SetActive(true);
            return false;
        }

        return true;
    }

    void HideAll()
    {
        namePopup.SetActive(false);
        noInternetPopup.SetActive(false);
    }
}
