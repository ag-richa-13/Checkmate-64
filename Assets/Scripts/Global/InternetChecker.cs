using UnityEngine;

public static class InternetChecker
{
    public static bool IsConnected()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
}
