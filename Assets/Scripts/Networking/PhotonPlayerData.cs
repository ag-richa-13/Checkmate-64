using Photon.Pun;
using Photon.Realtime;

public enum PlayerSide
{
    White,
    Black
}

public static class PhotonPlayerData
{
    public static bool IsHost { get; private set; }
    public static PlayerSide LocalSide { get; private set; }

    public static void SetHost()
    {
        IsHost = true;
    }

    public static void SetJoiner()
    {
        IsHost = false;
    }

    public static void Reset()
    {
        IsHost = false;
        LocalSide = PlayerSide.White;
    }

    // ---------- SIDE DECISION ----------

    public static void DecideSide()
    {
        LocalSide = PhotonNetwork.IsMasterClient
            ? PlayerSide.White
            : PlayerSide.Black;
    }

    // ---------- HELPERS ----------

    public static string LocalNickName =>
        PhotonNetwork.NickName;

    public static string WhitePlayerName =>
        PhotonNetwork.IsMasterClient
            ? PhotonNetwork.MasterClient.NickName
            : GetOtherPlayer().NickName;

    public static string BlackPlayerName =>
        PhotonNetwork.IsMasterClient
            ? GetOtherPlayer().NickName
            : PhotonNetwork.MasterClient.NickName;

    static Player GetOtherPlayer()
    {
        Player[] players = PhotonNetwork.PlayerList;
        return players[0] == PhotonNetwork.MasterClient
            ? players[1]
            : players[0];
    }
}
