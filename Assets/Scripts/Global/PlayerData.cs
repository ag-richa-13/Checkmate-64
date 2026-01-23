using UnityEngine;
using System;

public static class PlayerData
{
    private const string NAME_KEY = "PLAYER_NAME";
    private const string TIME_KEY = "PLAYER_NAME_TIME";

    private const double NAME_VALIDITY_SECONDS = 10 * 60;

    public static event Action OnNameUpdated;

    public static string Name
    {
        get
        {
            if (IsNameExpired())
            {
                ClearName();
                return string.Empty;
            }

            return PlayerPrefs.GetString(NAME_KEY, string.Empty);
        }
        set
        {
            PlayerPrefs.SetString(NAME_KEY, value);
            PlayerPrefs.SetString(TIME_KEY, GetCurrentTime());
            PlayerPrefs.Save();

            OnNameUpdated?.Invoke();
        }
    }

    public static bool HasName()
    {
        if (IsNameExpired())
        {
            ClearName();
            return false;
        }

        return !string.IsNullOrEmpty(PlayerPrefs.GetString(NAME_KEY, string.Empty));
    }

    static bool IsNameExpired()
    {
        if (!PlayerPrefs.HasKey(TIME_KEY))
            return true;

        DateTime savedTime = DateTime.Parse(
            PlayerPrefs.GetString(TIME_KEY),
            null,
            System.Globalization.DateTimeStyles.RoundtripKind
        );

        return (DateTime.UtcNow - savedTime).TotalSeconds >= NAME_VALIDITY_SECONDS;
    }

    static void ClearName()
    {
        PlayerPrefs.DeleteKey(NAME_KEY);
        PlayerPrefs.DeleteKey(TIME_KEY);
        PlayerPrefs.Save();
    }

    static string GetCurrentTime()
    {
        return DateTime.UtcNow.ToString("o");
    }
}
