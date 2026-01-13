using UnityEngine;

public static class PlayerData
{
    private const string KEY = "PLAYER_NAME";

    public static string Name
    {
        get => PlayerPrefs.GetString(KEY, "");
        set
        {
            PlayerPrefs.SetString(KEY, value);
            PlayerPrefs.Save();
        }
    }

    public static bool HasName()
    {
        return !string.IsNullOrEmpty(Name);
    }
}
