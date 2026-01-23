using UnityEngine;
using TMPro;

public class PlayerSlotUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text statusText;

    public void SetEmpty(string status, Color statusColor)
    {
        playerNameText.text = string.Empty;
        statusText.text = status;
        statusText.color = statusColor;
    }

    public void SetPlayer(string playerName, string status, Color statusColor)
    {
        playerNameText.text = playerName;
        statusText.text = status;
        statusText.color = statusColor;
    }
}
